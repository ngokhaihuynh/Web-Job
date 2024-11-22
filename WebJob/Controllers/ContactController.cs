using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now; // Ngày hiện tại
                model.CreatedBy = model.Name;
                db.Contacts.Add(model);
                
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cảm ơn bạn đã liên hệ. Chúng tôi sẽ trả lời bạn sớm!";
                return RedirectToAction("Contact");
            }
            return View(model);
        }
    }
}