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
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> items = db.products.OrderByDescending(x => x.ProductID);
            var pageSize = 10;
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
            ViewBag.ProductCategory = new SelectList(db.categoryProducts.ToList(), "CateProId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.Images = Images[i];
                            model.ProductImages.Add(new ProductImage
                            {
                                ProductID = model.ProductID,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            model.ProductImages.Add(new ProductImage
                            {
                                ProductID = model.ProductID,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = WebJob.Models.Common.Filter.FilterChar(model.Title);

                }
                db.products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.ProductCategory = new SelectList(db.categoryProducts.ToList(), "CateProId", "Title");
            return View(model);
        }
        public ActionResult Edit()
        {
            ViewBag.ProductCategory = new SelectList(db.categoryProducts.ToList(), "CateProId", "Title");
            return View();
        }
        // xin chào 1

    }
}