using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;

namespace ANU.Controllers
{
    public class ResultsController : Controller
    {
        public IActionResult Index()
        {
            return View(new ResultSearchViewModel());
        }

        [HttpPost]
        public IActionResult Index(ResultSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This would typically come from a database
                ViewBag.StudentName = "Ahmed Mohamed";
                ViewBag.SemesterGPA = "3.75";
                ViewBag.CumulativeGPA = "3.82";

                ViewBag.Results = new List<object>
                {
                    new { CourseCode = "CS301", CourseName = "Data Structures", Grade = "A", GPA = "4.0" },
                    new { CourseCode = "CS302", CourseName = "Algorithms", Grade = "A-", GPA = "3.7" },
                    new { CourseCode = "CS303", CourseName = "Database Systems", Grade = "B+", GPA = "3.3" }
                };
            }

            return View(model);
        }

        public IActionResult Details(string studentId, string semester)
        {
            // This would typically come from a database
            ViewBag.StudentName = "Ahmed Mohamed";
            ViewBag.Semester = semester;
            ViewBag.SemesterGPA = "3.75";
            ViewBag.CumulativeGPA = "3.82";

            var results = new List<Result>
            {
                new Result
                {
                    Id = 1,
                    CourseCode = "CS301",
                    CourseName = "Data Structures",
                    Grade = "A",
                    GPA = 4.0,
                    MidtermMark = 18,
                    MidtermTotal = 20,
                    AssignmentsMark = 28,
                    AssignmentsTotal = 30,
                    PracticalMark = 19,
                    PracticalTotal = 20,
                    FinalExamMark = 28,
                    FinalExamTotal = 30,
                    TotalMark = 93,
                    TotalPossible = 100
                },
                new Result
                {
                    Id = 2,
                    CourseCode = "CS302",
                    CourseName = "Algorithms",
                    Grade = "A-",
                    GPA = 3.7,
                    MidtermMark = 17,
                    MidtermTotal = 20,
                    AssignmentsMark = 27,
                    AssignmentsTotal = 30,
                    PracticalMark = 18,
                    PracticalTotal = 20,
                    FinalExamMark = 26,
                    FinalExamTotal = 30,
                    TotalMark = 88,
                    TotalPossible = 100
                },
                new Result
                {
                    Id = 3,
                    CourseCode = "CS303",
                    CourseName = "Database Systems",
                    Grade = "B+",
                    GPA = 3.3,
                    MidtermMark = 16,
                    MidtermTotal = 20,
                    AssignmentsMark = 25,
                    AssignmentsTotal = 30,
                    PracticalMark = 17,
                    PracticalTotal = 20,
                    FinalExamMark = 25,
                    FinalExamTotal = 30,
                    TotalMark = 83,
                    TotalPossible = 100
                }
            };

            return View(results);
        }
    }
}
