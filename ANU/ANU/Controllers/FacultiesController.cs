using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace ANU.Controllers
{
    public class FacultiesController : Controller
    {
        // STEP 1: Add a private field for the database context
        // private readonly ApplicationDbContext _context;

        // STEP 2: Inject the database context through constructor injection
        // public FacultiesController(ApplicationDbContext context)
        // {
        //     _context = context;
        // }

        public IActionResult Index()
        {
            // STEP 3: Replace hardcoded data with database query
            // This would typically come from a database
            // var faculties = await _context.Faculties.ToListAsync();

            var faculties = new List<Faculty>
            {
                new Faculty
                {
                    Id = 1,
                    Name = "Faculty of Computers & Artificial Intelligence",
                    Description = "We believe that software and information technology is the future, hence we have recruited our knowledge in the fields of information system and automation mechanisms to serve our beliefs."
                },
                new Faculty
                {
                    Id = 2,
                    Name = "Faculty of Medicine",
                    Description = "Faculty of Medicine at Assiut University stands out as a leading establishment for medical education in Upper Egypt"
                },
                new Faculty
                {
                    Id = 3,
                    Name = "Faculty of Engineering & Applied Sciences",
                    Description = "63 years of experience and excellence in Engineering academic fields including Electrical Engineering, Civil Engineering, Mechanical Engineering, Architectural Engineering, and Mining and Metallurgical Engineering"
                }
            };

            return View(faculties);
        }

        public IActionResult Details(int id)
        {
            // STEP 4: Replace hardcoded data with database query
            // This would typically come from a database
            // var faculty = await _context.Faculties
            //     .Include(f => f.Programs)
            //     .FirstOrDefaultAsync(f => f.Id == id);
            //
            // if (faculty == null)
            // {
            //     return NotFound();
            // }

            // Create faculty data based on the id parameter
            Faculty faculty;

            switch (id)
            {
                case 2:
                    faculty = new Faculty
                    {
                        Id = 2,
                        Name = "Faculty of Medicine",
                        Description = "Faculty of Medicine at Assiut University stands out as a leading establishment for medical education in Upper Egypt",
                        FullDescription = "The Faculty of Medicine at Assiut National University is dedicated to providing high-quality medical education and training future healthcare professionals. Our programs combine theoretical knowledge with practical clinical experience to prepare students for successful careers in medicine.",
                        ImageUrl = "/css/anu/faculty-medicine.jpg",
                        Programs = new List<AcademicProgram>
                        {
                            new AcademicProgram { Id = 4, Name = "Medicine" },
                            new AcademicProgram { Id = 5, Name = "Dentistry" },
                            new AcademicProgram { Id = 6, Name = "Pharmacy" }
                        }
                    };
                    break;
                case 3:
                    faculty = new Faculty
                    {
                        Id = 3,
                        Name = "Faculty of Engineering & Applied Sciences",
                        Description = "63 years of experience and excellence in Engineering academic fields including Electrical Engineering, Civil Engineering, Mechanical Engineering, Architectural Engineering, and Mining and Metallurgical Engineering",
                        FullDescription = "The Faculty of Engineering & Applied Sciences at Assiut National University offers comprehensive programs in various engineering disciplines. Our faculty members are experts in their fields and provide students with the knowledge and skills needed for successful careers in engineering.",
                        ImageUrl = "/css/anu/faculty-engineering.jpg",
                        Programs = new List<AcademicProgram>
                        {
                            new AcademicProgram { Id = 7, Name = "Electrical Engineering" },
                            new AcademicProgram { Id = 8, Name = "Civil Engineering" },
                            new AcademicProgram { Id = 9, Name = "Mechanical Engineering" },
                            new AcademicProgram { Id = 10, Name = "Architectural Engineering" }
                        }
                    };
                    break;
                default: // Default to Faculty of Computers & AI (id = 1)
                    faculty = new Faculty
                    {
                        Id = 1,
                        Name = "Faculty of Computers & Artificial Intelligence",
                        Description = "We believe that software and information technology is the future, hence we have recruited our knowledge in the fields of information system and automation mechanisms to serve our beliefs.",
                        FullDescription = "The Faculty of Computers & Artificial Intelligence at Assiut National University is dedicated to providing cutting-edge education in computer science, information technology, and artificial intelligence. Our programs are designed to prepare students for the rapidly evolving tech industry.",
                        ImageUrl = "/css/anu/faculty-computers.jpg",
                        Programs = new List<AcademicProgram>
                        {
                            new AcademicProgram { Id = 1, Name = "Computer Science" },
                            new AcademicProgram { Id = 2, Name = "Information Systems" },
                            new AcademicProgram { Id = 3, Name = "Artificial Intelligence" }
                        }
                    };
                    break;
            }

            return View(faculty);
        }
    }
}
