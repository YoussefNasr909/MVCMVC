using ANU.Models;
using System.Collections.Generic;

namespace ANU.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Add the missing EnrolledStudents property
        public virtual ICollection<User> EnrolledStudents { get; set; } = new List<User>();
    }
}