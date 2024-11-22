﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Subcrice()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Subscribe(EmailSubscription req)
        {
            if (ModelState.IsValid)
            {
                db.EmailSubscriptions.Add(new EmailSubscription { Email = req.Email, CreatedDate = DateTime.Now });
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return View("Partial_Subcrice", req);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Refresh()
        {
            var item = new ThongKeModel();
            ViewBag.Visitors_online = HttpContext.Application["Visitor_online"];
            item.HomNay = (HttpContext.Application["HomNay"]).ToString();
            item.HomQua = (HttpContext.Application["HomQua"]).ToString();
            item.TuanNay = (HttpContext.Application["TuanNay"]).ToString();
            item.TuanTruoc = (HttpContext.Application["TuanTruoc"]).ToString();
            item.ThangNay = (HttpContext.Application["ThangNay"]).ToString();
            item.ThangTruoc = (HttpContext.Application["ThangTruoc"]).ToString();
            item.TatCa = (HttpContext.Application["TatCa"]).ToString();
            


            return PartialView(item);
        }

      

    }
}