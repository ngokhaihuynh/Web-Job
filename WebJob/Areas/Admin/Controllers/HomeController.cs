using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Areas.Admin.Controllers
{
   [Authorize(Roles = "Admin")]
  
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);  // Tính toán ngày 7 ngày trước

            var newUsersCount = db.Users.Count(u => u.CreatedDate >= sevenDaysAgo);
            var newJobsCount = db.Jobs.Count(j => j.CreatedDate >= sevenDaysAgo);

            // Lấy số lượng phản hồi mới (chưa đọc)
            var newContactsCount = db.Contacts.Count(c => c.IsRead == false);

            var model = new DashboardStatsViewModel
            {
                NewUsers = newUsersCount,
                NewJobs = newJobsCount,
                NewContacts = newContactsCount
            };

            return View(model);
        }

        public ActionResult FeedbackListPartial()
        {
            // Lấy 5 công việc mới nhất
            var feedback = db.Contacts.Where(j => j.IsRead == false)
                .OrderByDescending(j => j.CreatedDate).Take(3).ToList();
            return PartialView("_FeedbackListPartial", feedback);
        }

    }
}