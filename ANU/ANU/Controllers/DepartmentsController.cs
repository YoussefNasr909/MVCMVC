using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace AssiutUniversity.Controllers
{
    public class DepartmentsController : Controller
    {
        // STEP 1: Add a private field for the database context
        // private readonly ApplicationDbContext _context;

        // STEP 2: Inject the database context through constructor injection
        // public DepartmentsController(ApplicationDbContext context)
        // {
        //     _context = context;
        // }

        public IActionResult Index(int? facultyId = null)
        {
            // STEP 3: Replace hardcoded data with database query
            // This would typically come from a database
            // var query = _context.Departments.AsQueryable();
            //
            // if (facultyId.HasValue)
            // {
            //     query = query.Where(d => d.FacultyId == facultyId.Value);
            //     
            //     // Get faculty name for display
            //     var faculty = await _context.Faculties.FindAsync(facultyId.Value);
            //     ViewBag.FacultyName = faculty?.Name ?? "Unknown Faculty";
            // }
            // else
            // {
            //     ViewBag.FacultyName = "All Faculties";
            // }
            //
            // var departments = await query.ToListAsync();

            var departments = new List<Department>
            {
                new Department
                {
                    Id = 1,
                    Name = "Computer Science",
                    Description = "The Computer Science department focuses on algorithms, programming languages, and software development.",
                    FacultyId = 1
                },
                new Department
                {
                    Id = 2,
                    Name = "Information Systems",
                    Description = "The Information Systems department focuses on database management, system analysis, and information technology infrastructure.",
                    FacultyId = 1
                },
                new Department
                {
                    Id = 3,
                    Name = "Artificial Intelligence",
                    Description = "The Artificial Intelligence department focuses on machine learning, neural networks, and intelligent systems.",
                    FacultyId = 1
                },
                new Department
                {
                    Id = 4,
                    Name = "Internal Medicine",
                    Description = "The Internal Medicine department focuses on the diagnosis and treatment of adult diseases.",
                    FacultyId = 2
                },
                new Department
                {
                    Id = 5,
                    Name = "Surgery",
                    Description = "The Surgery department focuses on surgical procedures and techniques.",
                    FacultyId = 2
                }
            };

            // Filter departments by faculty ID if provided
            if (facultyId.HasValue)
            {
                departments = departments.Where(d => d.FacultyId == facultyId.Value).ToList();
                // Set the faculty name for display
                ViewBag.FacultyName = facultyId == 1 ? "Faculty of Computers & Artificial Intelligence" :
                                     facultyId == 2 ? "Faculty of Medicine" :
                                     "Faculty of Engineering & Applied Sciences";
            }
            else
            {
                ViewBag.FacultyName = "All Faculties";
            }

            return View(departments);
        }

        public IActionResult Details(int id)
        {
            // STEP 4: Replace hardcoded data with database query
            // This would typically come from a database
            // var department = await _context.Departments
            //     .Include(d => d.Faculty)
            //     .FirstOrDefaultAsync(d => d.Id == id);
            //
            // if (department == null)
            // {
            //     return NotFound();
            // }

            // Create department data based on the id parameter
            Department department;

            switch (id)
            {
                case 2:
                    department = new Department
                    {
                        Id = 2,
                        Name = "Information Systems",
                        Description = "The Information Systems department focuses on database management, system analysis, and information technology infrastructure.",
                        FacultyId = 1
                    };
                    break;
                case 3:
                    department = new Department
                    {
                        Id = 3,
                        Name = "Artificial Intelligence",
                        Description = "The Artificial Intelligence department focuses on machine learning, neural networks, and intelligent systems.",
                        FacultyId = 1
                    };
                    break;
                case 4:
                    department = new Department
                    {
                        Id = 4,
                        Name = "Internal Medicine",
                        Description = "The Internal Medicine department focuses on the diagnosis and treatment of adult diseases.",
                        FacultyId = 2
                    };
                    break;
                case 5:
                    department = new Department
                    {
                        Id = 5,
                        Name = "Surgery",
                        Description = "The Surgery department focuses on surgical procedures and techniques.",
                        FacultyId = 2
                    };
                    break;
                default: // Default to Computer Science (id = 1)
                    department = new Department
                    {
                        Id = 1,
                        Name = "Computer Science",
                        Description = "The Computer Science department focuses on algorithms, programming languages, and software development.",
                        FacultyId = 1
                    };
                    break;
            }

            return View(department);
        }
    }
}
