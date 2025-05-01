using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ANU.Controllers
{
    public class SchedulesController : Controller
    {
        public IActionResult Index()
        {
            // This would typically come from a database
            var schedules = new List<Schedule>
            {
                new Schedule
                {
                    Id = 1,
                    Day = "Sunday",
                    TimeSlot = "09:00 - 10:30",
                    CourseId = 1,
                    CourseName = "Data Structures",
                    Location = "Room 101",
                    InstructorName = "Dr. Ahmed Hassan"
                },
                new Schedule
                {
                    Id = 2,
                    Day = "Sunday",
                    TimeSlot = "11:00 - 12:30",
                    CourseId = 2,
                    CourseName = "Algorithms",
                    Location = "Room 102",
                    InstructorName = "Dr. Mohamed Ali"
                },
                new Schedule
                {
                    Id = 3,
                    Day = "Monday",
                    TimeSlot = "09:00 - 10:30",
                    CourseId = 3,
                    CourseName = "Database Systems",
                    Location = "Room 103",
                    InstructorName = "Dr. Sara Ahmed"
                }
            };

            ViewBag.TimeSlots = new List<string>
            {
                "08:00 - 09:30",
                "09:30 - 11:00",
                "11:00 - 12:30",
                "12:30 - 14:00",
                "14:00 - 15:30",
                "15:30 - 17:00"
            };

            ViewBag.Days = new List<string>
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday"
            };

            return View(schedules);
        }

        public IActionResult Manage()
        {
            // Populate dropdowns
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

            ViewBag.TimeSlots = new List<string>
            {
                "08:00 - 09:30",
                "09:30 - 11:00",
                "11:00 - 12:30",
                "12:30 - 14:00",
                "14:00 - 15:30",
                "15:30 - 17:00"
            };

            ViewBag.Days = new List<string>
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday"
            };

            return View(new ScheduleViewModel());
        }

        [HttpPost]
        public IActionResult Manage(ScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process schedule management
                // Redirect to success page
                return RedirectToAction("Index");
            }

            // Repopulate dropdowns
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

            ViewBag.TimeSlots = new List<string>
            {
                "08:00 - 09:30",
                "09:30 - 11:00",
                "11:00 - 12:30",
                "12:30 - 14:00",
                "14:00 - 15:30",
                "15:30 - 17:00"
            };

            ViewBag.Days = new List<string>
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday"
            };

            return View(model);
        }
    }
}
