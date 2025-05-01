using Microsoft.AspNetCore.Mvc;
using ANU.Models;
using System;
using System.Collections.Generic;

namespace ANU.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index(int page = 1)
        {
            // This would typically come from a database
            var news = new List<NewsItem>
            {
                new NewsItem
                {
                    Id = 1,
                    Title = "New Computer Science Building Inaugurated",
                    Summary = "Assiut National University inaugurates a new state-of-the-art building for the Faculty of Computers & Artificial Intelligence.",
                    PublishedDate = DateTime.Now.AddDays(-5),
                    ImageUrl = "/css/anu/news1.jpg"
                },
                new NewsItem
                {
                    Id = 2,
                    Title = "University Hosts International Conference",
                    Summary = "Assiut National University hosts an international conference on emerging technologies and their applications.",
                    PublishedDate = DateTime.Now.AddDays(-10),
                    ImageUrl = "/css/anu/news2.jpg"
                },
                new NewsItem
                {
                    Id = 3,
                    Title = "Students Win National Programming Competition",
                    Summary = "A team of students from the Faculty of Computers & Artificial Intelligence wins first place in the national programming competition.",
                    PublishedDate = DateTime.Now.AddDays(-15),
                    ImageUrl = "/css/anu/news3.jpg"
                }
            };

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = 3;

            return View(news);
        }
    }
}
