using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System.Threading.Tasks;

namespace ANU.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the view model to the database model
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Subject = model.Subject,
                    Message = model.Message,
                    DateSubmitted = System.DateTime.UtcNow,
                    IsRead = false
                };

                // Add to database
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                // Show success message
                TempData["SuccessMessage"] = "Thank you for your message. We will get back to you soon.";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
