using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class ExperienceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Danh sách kinh nghiệm
        public ActionResult Index()
        {
            var experiences = db.Experiences.ToList();
            return View(experiences);
        }

        // Thêm mới
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Experience experience)
        {
            if (ModelState.IsValid)
            {
                if (!db.Experiences.Any(e => e.ExperienceLevel == experience.ExperienceLevel))
                {
                    db.Experiences.Add(experience);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Kinh nghiệm đã tồn tại.");
            }
            return View(experience);
        }

        // Sửa
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Experience experience = db.Experiences.Find(id);
            if (experience == null)
                return HttpNotFound();

            return View(experience);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Experience experience)
        {
            if (ModelState.IsValid)
            {
                db.Entry(experience).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(experience);
        }

        // Xóa
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Experience experience = db.Experiences.Find(id);
            if (experience != null)
            {
                if (!experience.Jobs.Any()) // Kiểm tra không có công việc liên kết
                {
                    db.Experiences.Remove(experience);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không thể xóa vì có công việc liên kết." });
            }
            return Json(new { success = false, message = "Không tìm thấy bản ghi." });
        }
    }
}
