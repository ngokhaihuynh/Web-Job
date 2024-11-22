using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebJob.Areas.Employer.Controllers
{
   
    public class HomeEmController : Controller
    {
        // GET: Employer/HomeEm
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}