using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class ArticelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articel
        public ActionResult Index(string alias)
        {
            var item = db.Articles.FirstOrDefault(x => x.Alias == alias);
            return View(item);
        }
    }
}