using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ANU.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index(int? facultyId, int? departmentId)
        {
            // Populate filter dropdowns
            ViewBag.Faculties = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Faculty of Computers & Artificial Intelligence" },
                new SelectListItem { Value = "2", Text = "Faculty of Medicine" },
                new SelectListItem { Value = "3", Text = "Faculty of Engineering & Applied Sciences" }
            };

            ViewBag.Departments = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Computer Science" },
                new SelectListItem { Value = "2", Text = "Information Systems" },
                new SelectListItem { Value = "3", Text = "Artificial Intelligence" }
            };

            ViewBag.SelectedFaculty = facultyId;
            ViewBag.SelectedDepartment = departmentId;

            // This would typically come from a database
            var staff = new List<Staff>
            {
                new Staff
                {
                    Id = 1,
                    FullName = "Dr. Ahmed Hassan",
                    Title = "Professor",
                    Department = "Computer Science",
                    Faculty = "Faculty of Computers & Artificial Intelligence",
                    ProfileImageUrl = "/css/anu/staff1.jpg"
                },
                new Staff
                {
                    Id = 2,
                    FullName = "Dr. Mohamed Ali",
                    Title = "Associate Professor",
                    Department = "Information Systems",
                    Faculty = "Faculty of Computers & Artificial Intelligence",
                    ProfileImageUrl = "/css/anu/staff2.jpg"
                },
                new Staff
                {
                    Id = 3,
                    FullName = "Dr. Sara Ahmed",
                    Title = "Assistant Professor",
                    Department = "Artificial Intelligence",
                    Faculty = "Faculty of Computers & Artificial Intelligence",
                    ProfileImageUrl = "/css/anu/staff3.jpg"
                }
            };

            return View(staff);
        }

        public IActionResult Details(int id)
        {
            // This would typically come from a database
            var staff = new Staff
            {
                Id = id,
                FullName = "Dr. Ahmed Hassan",
                Title = "Professor",
                Department = "Computer Science",
                Faculty = "Faculty of Computers & Artificial Intelligence",
                Education = "Ph.D. in Computer Science, MIT, 2010",
                ResearchInterests = new List<string>
                {
                    "Artificial Intelligence",
                    "Machine Learning",
                    "Data Mining"
                },
                CoursesTaught = new List<Course>
                {
                    new Course { Id = 1, Code = "CS301", Name = "Data Structures" },
                    new Course { Id = 2, Code = "CS302", Name = "Algorithms" }
                },
                Publications = new List<string>
                {
                    "Hassan, A., et al. (2020). 'Advanced Machine Learning Techniques for Big Data Analysis.' Journal of Computer Science, 15(2), 45-60.",
                    "Hassan, A., et al. (2019). 'Deep Learning Applications in Healthcare.' International Conference on Artificial Intelligence, pp. 120-135."
                },
                Email = "ahmed.hassan@assiut.edu",
                Office = "Building A, Room 101",
                OfficeHours = "Sunday and Tuesday, 10:00 - 12:00",
                ProfileImageUrl = "/css/anu/staff1.jpg"
            };

            return View(staff);
        }
    }
}
