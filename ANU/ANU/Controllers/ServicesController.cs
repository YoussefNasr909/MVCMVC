using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;

namespace ANU.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            // This would typically come from a database
            var services = new List<Service>
            {
                new Service
                {
                    Id = 1,
                    Name = "IT Support",
                    Description = "Technical assistance for students, faculty, and staff.",
                    ImageUrl = "/css/anu/it-support.jpg"
                },
                new Service
                {
                    Id = 2,
                    Name = "Digital Library",
                    Description = "Access to electronic resources, e-books, journals, and research papers.",
                    ImageUrl = "/css/anu/library.jpg"
                },
                new Service
                {
                    Id = 3,
                    Name = "Student Housing",
                    Description = "Comfortable and secure housing options for students.",
                    ImageUrl = "/css/anu/housing.jpg"
                }
            };

            return View(services);
        }

        public IActionResult ITSupport()
        {
            var service = new Service
            {
                Id = 1,
                Name = "IT Support",
                Description = "Technical assistance for students, faculty, and staff.",
                ImageUrl = "/css/anu/it-support.jpg"
            };

            return View(service);
        }

        public IActionResult DigitalLibrary()
        {
            var service = new Service
            {
                Id = 2,
                Name = "Digital Library",
                Description = "Access to electronic resources, e-books, journals, and research papers.",
                ImageUrl = "/css/anu/library.jpg"
            };

            return View(service);
        }

        public IActionResult Housing()
        {
            var service = new Service
            {
                Id = 3,
                Name = "Student Housing",
                Description = "Comfortable and secure housing options for students.",
                ImageUrl = "/css/anu/housing.jpg"
            };

            return View(service);
        }
    }
}
