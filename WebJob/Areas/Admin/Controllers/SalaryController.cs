using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class SalaryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Salary/Index
        public ActionResult Index(int? page)
        {
            int pageSize = 5; // Số bản ghi mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1

            // Lấy dữ liệu từ cơ sở dữ liệu và phân trang
            var salaries = db.Salaries.OrderBy(s => s.SalaryID).ToPagedList(pageNumber, pageSize);

            return View(salaries);
        }


        // GET: Salary/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Salary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salary salary)
        {
            if (ModelState.IsValid)
            {
                db.Salaries.Add(salary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salary);
        }

        // GET: Salary/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }

            return View(salary);
        }

        // POST: Salary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Salary salary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salary).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salary);
        }

        // GET: Salary/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var item = db.Salaries.Find(id);
                if (item != null)
                {
                    db.Salaries.Remove(item);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Xóa thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy bản ghi." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }



    }
}
