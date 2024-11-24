using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Areas.Admin.Controllers
{
    public class ContacttController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Contactt
        public ActionResult Index()
        {
            var contacts = db.Contacts.ToList();
            return View(contacts); // Truyền danh sách sang View
        }


        public ActionResult Details(int id)
        {
            var contact = db.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // Đổi trạng thái liên hệ
        [HttpPost]
        public JsonResult MarkAsRead(int id)
        {
            var contact = db.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contact.IsRead = true; // Đổi trạng thái thành Đã đọc
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Không tìm thấy liên hệ." });
        }



        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.Contacts.Find(id);
            if (item != null)
            {
                db.Contacts.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}