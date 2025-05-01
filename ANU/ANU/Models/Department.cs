using ANU.Models;

namespace ANU.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        // Add the missing ImageUrl property
        public string? ImageUrl { get; set; }
    }
}