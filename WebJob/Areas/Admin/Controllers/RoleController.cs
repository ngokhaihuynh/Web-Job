using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            var items = db.Roles.ToList();
            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        /*  public ActionResult Edit(int id)
          {
              var item = db.Roles.Find(id);
              return View();
          }*/

        /*   [HttpPost]
           [ValidateAntiForgeryToken]
           public ActionResult Edit(IdentityRole model)
           {
               if (ModelState.IsValid)
               {
                   var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                   roleManager.Update(model);
                   return RedirectToAction("Index");
               }
               return View(model);
           }*/

        public ActionResult Edit(string id)
        {
            var item = db.Roles.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var role = db.Roles.Find(model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        [HttpPost]
        public JsonResult Delete(string id)
        {
            var role = db.Roles.Find(id);
            if (role != null)
            {
                db.Roles.Remove(role);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        //public ActionResult Delete(int id)
        //{
        //    var item = db.Roles.Find(id);
        //    return View();
        //}
        public ActionResult Delete(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Delete(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}