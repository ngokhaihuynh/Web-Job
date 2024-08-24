using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductImages
        public ActionResult Index(int productId)
        {
            ViewBag.Product = productId;
            var items = db.productImages.Where(x => x.ProductImgID == productId).ToList();
           

            return View(items);
        }

    }
}