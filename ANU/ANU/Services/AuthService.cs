using ANU.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ANU.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Admin credentials
        private const string AdminEmail = "admin@gmail.com";
        private const string AdminPassword = "admin1";

        public AuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RegisterUserAsync(string email, string password, string firstName, string lastName)
        {
            // Check if user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return false;
            }

            // Create new user
            var user = new User
            {
                Email = email,
                PasswordHash = PasswordHasher.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                DateRegistered = DateTime.UtcNow,
                Role = "Student" // Default role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            // Check if this is the admin user
            if (email == AdminEmail && password == AdminPassword)
            {
                // Create or get admin user
                var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == AdminEmail);

                if (adminUser == null)
                {
                    // Create admin user if it doesn't exist
                    adminUser = new User
                    {
                        Email = AdminEmail,
                        PasswordHash = PasswordHasher.HashPassword(AdminPassword),
                        FirstName = "Admin",
                        LastName = "User",
                        DateRegistered = DateTime.UtcNow,
                        Role = "Admin"
                    };

                    _context.Users.Add(adminUser);
                    await _context.SaveChangesAsync();
                }
                else if (adminUser.Role != "Admin")
                {
                    // Ensure the user has admin role
                    adminUser.Role = "Admin";
                    _context.Users.Update(adminUser);
                    await _context.SaveChangesAsync();
                }

                return adminUser;
            }

            // Regular user authentication
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            return isPasswordValid ? user : null;
        }

        public async Task SignInAsync(User user, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddHours(2)
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return null;
            }

            return await _context.Users.FindAsync(id);
        }

        public bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }

        // Add the missing IsAdminAsync method
        public async Task<bool> IsAdminAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.Role == "Admin";
        }
    }
}