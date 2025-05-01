using System.Collections.Generic;

namespace ANU.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Day { get; set; } = string.Empty;
        public string TimeSlot { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
    }

    public class ScheduleViewModel
    {
        public int FacultyId { get; set; }
        public int DepartmentId { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; } = string.Empty;
        public List<Schedule> Schedule { get; set; } = new List<Schedule>();
    }
}
