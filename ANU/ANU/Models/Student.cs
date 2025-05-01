using ANU.Models;
using System.Collections.Generic;

namespace ANU.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int Year { get; set; }
        public string ProfileImageUrl { get; set; } = string.Empty;
        public List<Course> EnrolledCourses { get; set; } = new List<Course>();
    }

    public class StudentRegistration
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public int FacultyId { get; set; }
    }
}
