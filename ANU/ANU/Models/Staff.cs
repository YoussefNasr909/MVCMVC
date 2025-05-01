using ANU.Models;
using System.Collections.Generic;

namespace ANU.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public List<string> ResearchInterests { get; set; } = new List<string>();
        public List<Course> CoursesTaught { get; set; } = new List<Course>();
        public List<string> Publications { get; set; } = new List<string>();
        public string Email { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
        public string OfficeHours { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}
