using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: News
        public ActionResult Index()
        {
            var items = db.News.ToList(); 
            return View(items);
        }

        public ActionResult Detail(int id )
        {
            var item = db.News.Find(id);
            return View(item);
        }

        public ActionResult Partial_LatestNews()
        {
            var latestNews = db.News
                               .OrderByDescending(n => n.CreatedDate) // Sắp xếp theo ngày tạo giảm dần
                               .Take(5) // Lấy 5 tin tức mới nhất
                               .ToList();

            return PartialView(latestNews); // Trả về PartialView
        }

        public ActionResult Partial_News_Home()
        {
            var items = db.News.Take(3).ToList();
            return PartialView(items);
        }
    }
}