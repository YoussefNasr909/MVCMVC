using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ANU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            return View();
        }

        public async Task<IActionResult> About()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            // Load data for navbar
            ViewBag.AllFaculties = await _context.Faculties.ToListAsync();
            ViewBag.AllDepartments = await _context.Departments.ToListAsync();

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}