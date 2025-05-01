using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Threading.Tasks;
using ANU.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ANU.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;

        public AccountController(AuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        public async Task<IActionResult> Login()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            // If user is already logged in, redirect to appropriate page
            if (User.Identity?.IsAuthenticated == true)
            {
                if (await _authService.IsAdminAsync())
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            if (ModelState.IsValid)
            {
                var user = await _authService.AuthenticateAsync(model.Email, model.Password);
                if (user != null)
                {
                    // Set persistent cookie only if RememberMe is checked
                    await _authService.SignInAsync(user, model.RememberMe);

                    // Add welcome message
                    TempData["SuccessMessage"] = $"Welcome, {user.FirstName}!";

                    // Redirect to admin dashboard if user is admin
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ViewData["LoginError"] = "Invalid login attempt. Please check your email and password.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            if (ModelState.IsValid)
            {
                bool success = await _authService.RegisterUserAsync(
                    model.Email,
                    model.Password,
                    model.FirstName,
                    model.LastName);

                if (success)
                {
                    // Log the user in after registration
                    var user = await _authService.AuthenticateAsync(model.Email, model.Password);
                    if (user != null)
                    {
                        await _authService.SignInAsync(user, false); // Non-persistent by default

                        // Add welcome message
                        TempData["SuccessMessage"] = $"Welcome, {user.FirstName}! Your account has been created successfully.";

                        return RedirectToAction("Profile", "Students");
                    }
                }
                else
                {
                    ViewData["RegisterError"] = "Email is already in use. Please try a different email address.";
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AccessDenied()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            return View();
        }
    }
}