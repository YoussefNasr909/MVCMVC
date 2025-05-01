using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ANU.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index(string? category = null, int page = 1)
        {
            // This would typically come from a database
            var posts = new List<BlogPost>
            {
                new BlogPost
                {
                    Id = 1,
                    Title = "The Future of Artificial Intelligence",
                    Summary = "Exploring the potential impact of AI on various industries and society as a whole.",
                    Author = "Dr. Ahmed Hassan",
                    Category = "Technology",
                    PublishedDate = DateTime.Now.AddDays(-5),
                    ImageUrl = "/css/anu/blog1.jpg"
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Student Life at Assiut National University",
                    Summary = "A glimpse into the vibrant student life and activities at our university.",
                    Author = "Sarah Mohamed",
                    Category = "Campus Life",
                    PublishedDate = DateTime.Now.AddDays(-10),
                    ImageUrl = "/css/anu/blog2.jpg"
                },
                new BlogPost
                {
                    Id = 3,
                    Title = "Advancements in Medical Research",
                    Summary = "Recent breakthroughs in medical research conducted at our Faculty of Medicine.",
                    Author = "Dr. Mohamed Ali",
                    Category = "Research",
                    PublishedDate = DateTime.Now.AddDays(-15),
                    ImageUrl = "/css/anu/blog3.jpg"
                }
            };

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                posts = posts.Where(p => p.Category == category).ToList();
            }

            ViewBag.Categories = new List<string> { "Technology", "Campus Life", "Research", "Events" };
            ViewBag.Category = category;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = 3;

            return View(posts);
        }
    }
}
