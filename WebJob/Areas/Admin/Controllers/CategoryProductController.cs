using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class CategoryProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/CategoryProduct
        public ActionResult Index()
        {
            var items = db.categoryProducts;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebJob.Models.Common.Filter.FilterChar(model.Title);
                db.categoryProducts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.categoryProducts.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                var item = db.categoryProducts.Find(model.CateProId);
                if (item != null)
                {
                    item.Title = model.Title;
                    item.Description = model.Description;
                    item.Alias = WebJob.Models.Common.Filter.FilterChar(model.Title);
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedBy = model.ModifiedBy;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy danh mục sản phẩm.");
                }
            }
            return View(model);
        }


        public ActionResult Delete(int id)
        {
            var item = db.categoryProducts.Find(id);
            if (item != null)
            {
                var DeleteItem = db.categoryProducts.Attach(item);
                db.categoryProducts.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}