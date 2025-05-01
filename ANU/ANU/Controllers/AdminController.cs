using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ANU.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ANU.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context, AuthService authService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var currentUser = await _authService.GetCurrentUserAsync();
            ViewBag.AdminName = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "Admin User";
            ViewBag.StudentCount = await _context.Users.CountAsync(u => u.Role == "Student");
            ViewBag.FacultyCount = await _context.Faculties.CountAsync();
            ViewBag.CourseCount = await _context.Courses.CountAsync();
            ViewBag.StaffCount = await _context.Staff.CountAsync();

            return View();
        }

        #region Staff Management
        public async Task<IActionResult> ManageStaff()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var staffList = await _context.Staff.ToListAsync();
            return View(staffList);
        }

        public async Task<IActionResult> CreateStaff()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStaff(Staff staff, IFormFile ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "staff");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid() + "_" + ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }
                    staff.ProfileImageUrl = "/uploads/staff/" + uniqueFileName;
                }

                _context.Staff.Add(staff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Staff member created successfully.";
                return RedirectToAction(nameof(ManageStaff));
            }

            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(staff);
        }

        public async Task<IActionResult> EditStaff(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) return NotFound();

            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStaff(int id, Staff staff, IFormFile ProfileImage)
        {
            if (id != staff.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingStaff = await _context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingStaff.ProfileImageUrl))
                    {
                        var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, existingStaff.ProfileImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "staff");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid() + "_" + ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }
                    staff.ProfileImageUrl = "/uploads/staff/" + uniqueFileName;
                }
                else
                {
                    staff.ProfileImageUrl = existingStaff.ProfileImageUrl;
                }

                _context.Update(staff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Staff member updated successfully.";
                return RedirectToAction(nameof(ManageStaff));
            }

            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(staff);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var staff = await _context.Staff.Include(s => s.Department).FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        [HttpPost, ActionName("DeleteStaff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStaffConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) return NotFound();

            if (!string.IsNullOrEmpty(staff.ProfileImageUrl))
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, staff.ProfileImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Staff member deleted successfully.";
            return RedirectToAction(nameof(ManageStaff));
        }

        private bool StaffExists(int id) => _context.Staff.Any(e => e.Id == id);
        #endregion

        #region Faculties Management
        public async Task<IActionResult> ManageFaculties()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            var list = await _context.Faculties.ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> CreateFaculty()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaculty(Faculty faculty, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "faculties");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fs);
                    }
                    faculty.ImageUrl = "/uploads/faculties/" + fname;
                }

                _context.Faculties.Add(faculty);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Faculty created successfully.";
                return RedirectToAction(nameof(ManageFaculties));
            }
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            return View(faculty);
        }

        public async Task<IActionResult> EditFaculty(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var fac = await _context.Faculties.FindAsync(id);
            if (fac == null) return NotFound();
            return View(fac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaculty(int id, Faculty faculty, IFormFile ImageFile)
        {
            if (id != faculty.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Faculties.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var old = Path.Combine(_webHostEnvironment.WebRootPath, existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "faculties");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fs);
                    }
                    faculty.ImageUrl = "/uploads/faculties/" + fname;
                }
                else
                {
                    faculty.ImageUrl = existing.ImageUrl;
                }

                _context.Update(faculty);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Faculty updated successfully.";
                return RedirectToAction(nameof(ManageFaculties));
            }
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            return View(faculty);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null) return NotFound();
            return View(faculty);
        }

        [HttpPost, ActionName("DeleteFaculty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFacultyConfirmed(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null) return NotFound();

            if (!string.IsNullOrEmpty(faculty.ImageUrl))
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, faculty.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Faculty deleted successfully.";
            return RedirectToAction(nameof(ManageFaculties));
        }

        private bool FacultyExists(int id) => _context.Faculties.Any(e => e.Id == id);
        #endregion

        #region Departments Management
        public async Task<IActionResult> ManageDepartments()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var list = await _context.Departments.Include(d => d.Faculty).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> CreateDepartment()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(Department department, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "departments");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fs);
                    }
                    department.ImageUrl = "/uploads/departments/" + fname;
                }

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department created successfully.";
                return RedirectToAction(nameof(ManageDepartments));
            }
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            return View(department);
        }

        public async Task<IActionResult> EditDepartment(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(int id, Department department, IFormFile ImageFile)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var old = Path.Combine(_webHostEnvironment.WebRootPath, existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "departments");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fs);
                    }
                    department.ImageUrl = "/uploads/departments/" + fname;
                }
                else
                {
                    department.ImageUrl = existing.ImageUrl;
                }

                _context.Update(department);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department updated successfully.";
                return RedirectToAction(nameof(ManageDepartments));
            }
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var department = await _context.Departments.Include(d => d.Faculty).FirstOrDefaultAsync(m => m.Id == id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost, ActionName("DeleteDepartment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartmentConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            if (!string.IsNullOrEmpty(department.ImageUrl))
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, department.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Department deleted successfully.";
            return RedirectToAction(nameof(ManageDepartments));
        }

        private bool DepartmentExists(int id) => _context.Departments.Any(e => e.Id == id);
        #endregion

        #region Users Management
        public async Task<IActionResult> ManageUsers()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var users = await _context.Users.Include(u => u.Faculty).Include(u => u.Department).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> CreateUser()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user, string password, IFormFile ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    ViewBag.Faculties = await _context.Faculties.ToListAsync();
                    ViewBag.Departments = await _context.Departments.ToListAsync();
                    return View(user);
                }

                user.PasswordHash = PasswordHasher.HashPassword(password);
                user.DateRegistered = DateTime.UtcNow;

                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ProfileImage.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fs);
                    }
                    user.ProfileImageUrl = "/uploads/profiles/" + fname;
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction(nameof(ManageUsers));
            }

            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(user);
        }

        public async Task<IActionResult> EditUser(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, User user, string newPassword, IFormFile ProfileImage)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

                user.PasswordHash = !string.IsNullOrEmpty(newPassword)
                    ? PasswordHasher.HashPassword(newPassword)
                    : existingUser.PasswordHash;

                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingUser.ProfileImageUrl))
                    {
                        var old = Path.Combine(_webHostEnvironment.WebRootPath, existingUser.ProfileImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ProfileImage.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fs);
                    }
                    user.ProfileImageUrl = "/uploads/profiles/" + fname;
                }
                else
                {
                    user.ProfileImageUrl = existingUser.ProfileImageUrl;
                }

                user.DateRegistered = existingUser.DateRegistered;

                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User updated successfully.";
                return RedirectToAction(nameof(ManageUsers));
            }

            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Faculties = await _context.Faculties.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var user = await _context.Users
                .Include(u => u.Faculty)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfileImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(ManageUsers));
        }

        private bool UserExists(int id) => _context.Users.Any(e => e.Id == id);
        #endregion

        #region News Management
        public async Task<IActionResult> ManageNews()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var news = await _context.NewsItems.ToListAsync();
            return View(news);
        }

        public async Task<IActionResult> CreateNews()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNews(NewsItem news, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "news");
                    Directory.CreateDirectory(uploadsFolder);
                    string fname = Guid.NewGuid() + "_" + ImageFile.FileName;
                    string fpath = Path.Combine(uploadsFolder, fname);
                    using (var fs = new FileStream(fpath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fs);
                    }
                    news.ImageUrl = "/uploads/news/" + fname;
                }

                if (news.PublishedDate == default) news.PublishedDate = DateTime.Now;

                _context.NewsItems.Add(news);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "News item created successfully.";
                return RedirectToAction(nameof(ManageNews));
            }

            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            return View(news);
        }
        #endregion

        #region Courses Management
        public async Task<IActionResult> ManageCourses()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var courses = await _context.Courses.Include(c => c.Department).ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> CreateCourse()
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course created successfully.";
                return RedirectToAction(nameof(ManageCourses));
            }
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(course);
        }

        public async Task<IActionResult> EditCourse(int id)
        {
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, Course course)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Course updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Courses.Any(c => c.Id == course.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(ManageCourses));
            }
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course deleted successfully.";
            return RedirectToAction(nameof(ManageCourses));
        }
        #endregion
    }
}
