using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ANU.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace ANU.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentsController(ApplicationDbContext context, AuthService authService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Profile()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            // Load related entities
            var user = await _context.Users
                .Include(u => u.Faculty)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

            // Get enrolled courses
            var enrolledCourses = await _context.Courses
                .Where(c => c.EnrolledStudents.Any(s => s.Id == user.Id))
                .ToListAsync();

            ViewBag.EnrolledCourses = enrolledCourses;

            return View(user);
        }

        public async Task<IActionResult> EditProfile()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(User user, string newPassword, IFormFile ProfileImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);

                    // Only update password if a new one is provided
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        user.PasswordHash = PasswordHasher.HashPassword(newPassword);
                    }
                    else
                    {
                        user.PasswordHash = existingUser.PasswordHash;
                    }

                    // Handle profile image upload
                    if (ProfileImage != null && ProfileImage.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingUser.ProfileImageUrl))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingUser.ProfileImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                        Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfileImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ProfileImage.CopyToAsync(fileStream);
                        }

                        user.ProfileImageUrl = "/uploads/profiles/" + uniqueFileName;
                    }
                    else
                    {
                        // Keep existing image
                        user.ProfileImageUrl = existingUser.ProfileImageUrl;
                    }

                    // Preserve role and registration date
                    user.Role = existingUser.Role;
                    user.DateRegistered = existingUser.DateRegistered;

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                    return RedirectToAction("Profile");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Users.AnyAsync(u => u.Id == user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(user);
        }

        public async Task<IActionResult> ManageCourses()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            // Get enrolled courses
            var enrolledCourses = await _context.Courses
                .Include(c => c.Department)
                .Where(c => c.EnrolledStudents.Any(s => s.Id == currentUser.Id))
                .ToListAsync();

            // Get available courses for the student's department
            var availableCourses = new List<Course>();
            if (currentUser.DepartmentId.HasValue)
            {
                availableCourses = await _context.Courses
                    .Include(c => c.Department)
                    .Where(c => c.DepartmentId == currentUser.DepartmentId && !c.EnrolledStudents.Any(s => s.Id == currentUser.Id))
                    .ToListAsync();
            }

            ViewBag.EnrolledCourses = enrolledCourses;
            ViewBag.AvailableCourses = availableCourses;
            ViewBag.EnrolledCourseIds = enrolledCourses.Select(c => c.Id).ToList();

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollCourse(int courseId)
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            // Check if already enrolled
            var isEnrolled = await _context.Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.EnrolledStudents)
                .AnyAsync(s => s.Id == currentUser.Id);

            if (!isEnrolled)
            {
                // Add the student to the course
                course.EnrolledStudents = course.EnrolledStudents ?? new List<User>();
                course.EnrolledStudents.Add(currentUser);

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully enrolled in {course.Name}.";
            }

            return RedirectToAction("ManageCourses");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DropCourse(int courseId)
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.EnrolledStudents)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound();
            }

            var student = course.EnrolledStudents?.FirstOrDefault(s => s.Id == currentUser.Id);
            if (student != null)
            {
                course.EnrolledStudents.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully dropped {course.Name}.";
            }

            return RedirectToAction("ManageCourses");
        }
    }
}