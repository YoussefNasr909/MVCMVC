using System.Collections.Generic;

namespace ANU.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<AcademicProgram> Programs { get; set; } = new List<AcademicProgram>();
    }

    // Renamed from Program to AcademicProgram to avoid conflict with the entry point Program class
    public class AcademicProgram
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
