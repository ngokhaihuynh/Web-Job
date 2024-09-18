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
            
            return View();
        }

        public ActionResult View_itemsByCatePdId(int cateid)
        {
            var items = db.products.Where(x => x.CateProId == cateid).ToList();
            return PartialView(items);
        }

    }
}