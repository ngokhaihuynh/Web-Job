using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult MenuTop()
        //{
        //    var items = db.Categories.OrderBy(x => x.Position).ToList();
        //    return PartialView("_MenuTop", items);
        //}

            public ActionResult MenuTop()
            {
                // Lấy danh mục dành cho Ứng viên và danh mục cho cả 2, sắp xếp theo Position
                var items = db.Categories
                              .Where(x => x.CategoryType == CategoryType.Candidate || x.CategoryType == CategoryType.Both)
                              .OrderBy(x => x.Position)
                              .ToList();

                return PartialView("_MenuTop", items);
            }



        public ActionResult MenuJobCategory()
        {
            var items = db.JobCategories.ToList();
            return PartialView("_MenuJobCategory", items);
        }

        public ActionResult MenuJobLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }

            var items = db.JobCategories.ToList();
            var Alljob = db.Jobs.Count();
            ViewBag.All = Alljob;
            return PartialView("_MenuJobLeft", items);
        }

        //public ActionResult MenuProductLeft(int? id)
        //{
        //    if (id != null)
        //    {
        //        ViewBag.CateProId = id;
        //    }

        //    var items = db.categoryProducts.ToList();
        //    var AllProduct = db.products.Count();
        //    ViewBag.All = AllProduct;
        //    return PartialView("_MenuProductLeft", items);
        //}

        //public ActionResult MenuCategory()
        //{
        //    var items = db.JobCategories.ToList();
        //    return View(items);
        //}


        // PRODUCT
        public ActionResult MenuProductCategory()
        {
            var items = db.categoryProducts.ToList();
            return PartialView("_MenuProductCategory", items);
        }

        public ActionResult MenuArrivals()
        {
            var items = db.categoryProducts.ToList();
            return PartialView("_MenuArrivals", items);
        }

        public ActionResult MenuProductLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.categoryProducts.ToList();
            return PartialView("_MenuProductLeft", items);
        }

    }
}