using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ANU.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Custom user properties
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public string ProfileImageUrl { get; set; } = string.Empty;

        // Optional: Faculty and Department relationships
        public int? FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // For students
        public string? StudentId { get; set; }
        public int? Year { get; set; }

        // For staff
        public string? StaffId { get; set; }
        public string? Title { get; set; }
        public string? Office { get; set; }
        public string? OfficeHours { get; set; }
    }
}
