using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index(int? id)
        {
            // Bắt đầu với danh sách công việc đang hoạt động
            var query = db.Jobs.Where(x => x.IsActive);

            // Nếu có id hợp lệ, lọc theo danh mục công việc
            if (id > 0)
            {
                query = query.Where(x => x.JobCategoryID == id);
            }

            // Chuyển đổi query thành danh sách
            var items = query.ToList();

            // Cung cấp thông tin danh mục cho view
            ViewBag.CateId = id;

            return View(items);
        }

        public ActionResult Detail(string alias, int id)
        {
            var item = db.Jobs.Find(id);
            // lấy công việc tương tự
            return View(item);
        }

        public ActionResult JobCategory(string alias, int id)
        {
            // Bắt đầu với danh sách công việc đang hoạt động
            var query = db.Jobs.Where(x => x.IsActive);

            // Nếu có id hợp lệ, lọc theo danh mục công việc
            if (id > 0)
            {
                query = query.Where(x => x.JobCategoryID == id);
            }

            // Chuyển đổi query thành danh sách
            var items = query.ToList();

            // Cung cấp thông tin danh mục cho view
            ViewBag.CateId = id;
            ViewBag.JobCount = items.Count;
            return View(items);
        }


        public ActionResult View_itemsByCateId(string alias, int? id)
        {
            // Tìm kiếm danh mục công việc dựa trên alias
            var category = db.JobCategories.FirstOrDefault(c => c.Alias == alias);
            if (category == null)
            {
                return HttpNotFound(); // Trả về 404 nếu không tìm thấy danh mục công việc
            }

            // Nếu id không phải null và khớp với danh mục công việc, tìm kiếm các công việc
            if (id.HasValue && id.Value == category.JobCategoryID && category.IsActive)
            {
                var items = db.Jobs.Where(x => x.JobCategoryID == category.JobCategoryID && x.IsActive).ToList();
                ViewBag.JobCount = items.Count;
                return View(items); // Trả về view với danh sách công việc
            }

            return HttpNotFound(); // Trả về 404 nếu không có id khớp
        }


        // load Việc tuyển gấp
        public ActionResult Partial_JobNow()
        {
            var items = db.Jobs.Where(x => x.IsActive && x.IsNow).Take(10).ToList();
            return PartialView(items);
        }
    }
}