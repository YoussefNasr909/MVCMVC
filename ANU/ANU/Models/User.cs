using System;
using System.ComponentModel.DataAnnotations;

namespace ANU.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

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

        // Role (Admin, Student, Staff, etc.)
        public string Role { get; set; } = "Student";
    }
}
