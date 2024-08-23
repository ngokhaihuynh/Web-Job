using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class CompanyImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/CompanyImages
        public ActionResult Index(int? id)
        {
            ViewBag.JobID = id;
            var items = db.CompanyImages.Where(x => x.JobID == id).ToList();
            ViewBag.CompanyID = items.FirstOrDefault()?.CompanyID ?? 0;

            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int companyImgId, string url, int companyId, int jobId)
        {

            db.CompanyImages.Add(new CompanyImage
            {
                CompanyImageID = companyImgId,
                CompanyID = companyId,
                JobID = jobId,
                ImageURL = url,
                IsDefault = false
            });
            db.SaveChanges();
            return Json(new { success = true });
             

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.CompanyImages.Find(id);
            db.CompanyImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}