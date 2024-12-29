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
    [Authorize(Roles = "Admin")]
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Jobs
        public ActionResult Index(int? page)
        {
            IEnumerable<Job> items = db.Jobs.OrderByDescending(x => x.JobID);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Job model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                             
                            model.CompanyImages.Add(new CompanyImage
                            {
                                JobID = model.JobID,
                                ImageURL = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            model.CompanyImages.Add(new CompanyImage
                            {
                                JobID = model.JobID,
                                ImageURL = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now; 
                model.ModifiedDate = DateTime.Now; 

                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = WebJob.Models.Common.Filter.FilterChar(model.JobTitle);

                }
                db.Jobs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
            var items = db.Jobs.Find(id);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Job model)
        {
            if (ModelState.IsValid)
            {
                // Tìm bản ghi Job trong cơ sở dữ liệu dựa trên JobID
                var jobInDb = db.Jobs.Find(model.JobID);
                if (jobInDb == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy bản ghi cần chỉnh sửa.");
                    return View(model);
                }

                // Cập nhật các thuộc tính cần thiết, trừ CompanyID
                jobInDb.JobTitle = model.JobTitle;
                jobInDb.JobCategoryID = model.JobCategoryID;
                jobInDb.ModifiedDate = DateTime.Now;
                jobInDb.Alias = WebJob.Models.Common.Filter.FilterChar(model.JobTitle);
                jobInDb.JobDescription = model.JobDescription;
                jobInDb.IsActive = model.IsActive;
                jobInDb.IsNow = model.IsNow;
                jobInDb.EndDate = model.EndDate;
                jobInDb.Company.CompanyEmail = model.Company.CompanyEmail;
                jobInDb.JobRequirements = model.JobRequirements;
                //jobInDb.Salary.SalaryRange = model.Salary.SalaryRange;
                jobInDb.Salary.SalaryMin = model.Salary.SalaryMin;
                jobInDb.Salary.SalaryMax = model.Salary.SalaryMax;
                jobInDb.Experience.ExperienceLevel = model.Experience.ExperienceLevel;
                jobInDb.Location.LocationName = model.Location.LocationName;
                
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Khôi phục ViewBag và trả về view nếu ModelState không hợp lệ
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName", model.JobCategoryID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.Jobs.Find(id);
            if (item != null)
            {
                db.Jobs.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult isActive(int id)
        {

            var item = db.Jobs.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });

            }
            return Json(new { success = false });
        }[HttpPost]
        public ActionResult IsNow(int id)
        {

            var item = db.Jobs.Find(id);
            if (item != null)
            {
                item.IsNow = !item.IsNow;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsNow = item.IsNow });

            }
            return Json(new { success = false });
        }

    }
}