using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class SkillController : Controller
    {
        private  ApplicationDbContext db = new ApplicationDbContext();

        // Danh sách JobSkill
        public ActionResult Index(int? page)
        {
            // Số lượng bản ghi trên mỗi trang
            int pageSize = 8;

            // Trang hiện tại (mặc định là 1)
            int pageNumber = page ?? 1;

            // Lấy danh sách kỹ năng không trùng nhau theo JobSkillName
            var jobSkills = db.JobSkills
                              .GroupBy(js => js.JobSkillName)
                              .Select(g => g.FirstOrDefault())
                              .OrderBy(js => js.JobSkillName) // Sắp xếp theo tên kỹ năng
                              .ToPagedList(pageNumber, pageSize);

            return View(jobSkills);
        }



        // Thêm mới JobSkill (GET)
        public ActionResult Create()
        {
            return View();
        }

        // Thêm mới JobSkill (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobSkill model)
        {
            if (ModelState.IsValid)
            {
                db.JobSkills.Add(model);
                db.SaveChanges();
                TempData["Success"] = "Thêm mới thành công!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Thêm mới thất bại!";
            return View(model);
        }

        // Sửa JobSkill (GET)
        public ActionResult Edit(int id)
        {
            var jobSkill = db.JobSkills.Find(id);
            if (jobSkill == null)
            {
                return HttpNotFound();
            }
            return View(jobSkill);
        }

        // Sửa JobSkill (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobSkill model)
        {
            if (ModelState.IsValid)
            {
                var existingJobSkill = db.JobSkills.Find(model.JobSkillID);
                if (existingJobSkill != null)
                {
                    existingJobSkill.JobSkillName = model.JobSkillName;
                    existingJobSkill.JobID = model.JobID;

                    db.SaveChanges();
                    TempData["Success"] = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Không tìm thấy bản ghi!";
            }
            TempData["Error"] = "Cập nhật thất bại!";
            return View(model);
        }

        // Xóa JobSkill
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var jobSkill = db.JobSkills.Find(id);
            if (jobSkill != null)
            {
                db.JobSkills.Remove(jobSkill);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công!" });
            }
            return Json(new { success = false, message = "Xóa thất bại, bản ghi không tồn tại!" });
        }
    }
}