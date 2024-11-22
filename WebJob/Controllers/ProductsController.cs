using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index(int? id)
        {
            var items = db.products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.CateProId == id).ToList();
            }
            return View(items);
        }

    
        public ActionResult DetailProduct(string alias, int id)
        {

            var item = db.products.Find(id);
            var defaultImages = item.ProductImages.Where(x => x.IsDefault).ToList();
            ViewBag.DefaultImages = defaultImages;
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.CateProId == id).ToList();
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult View_itemsByCatePdId(int page = 1, int pageSize = 5)
        {
            var items = db.products.Where(x => x.IsActive).OrderBy(x => x.ProductID).ToPagedList(page, pageSize);
            return PartialView(items);
        }


    }
}