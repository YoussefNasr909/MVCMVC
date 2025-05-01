namespace ANU.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public double GPA { get; set; }
        public int MidtermMark { get; set; }
        public int MidtermTotal { get; set; }
        public int AssignmentsMark { get; set; }
        public int AssignmentsTotal { get; set; }
        public int PracticalMark { get; set; }
        public int PracticalTotal { get; set; }
        public int FinalExamMark { get; set; }
        public int FinalExamTotal { get; set; }
        public int TotalMark { get; set; }
        public int TotalPossible { get; set; }
    }

    public class ResultSearchViewModel
    {
        public string StudentId { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
    }
}
