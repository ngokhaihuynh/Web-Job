using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductImages
        public ActionResult Index(int? id)
        {
            ViewBag.ProductId = id;
            var items = db.productImages.Where(x => x.ProductID == id).ToList();
            return View(items);
        }


        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {

            db.productImages.Add(new ProductImage
            {
                ProductID = productId,
                Image = url,
                IsDefault = false
            });
            db.SaveChanges();
            return Json(new { success = true });


        }

        [HttpPost]
        public ActionResult Delete (int id)
        {
            var item = db.productImages.Find(id);
            db.productImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }

        // Cập nhật ảnh đại diện (IsDefault)
        [HttpPost]
        public ActionResult SetDefaultImage(int id)
        {
            // Tìm ảnh hiện tại đang là ảnh đại diện
            var currentDefaultImage = db.productImages.FirstOrDefault(pi => pi.IsDefault);

            // Nếu có ảnh đang là đại diện, hủy chọn nó
            if (currentDefaultImage != null)
            {
                currentDefaultImage.IsDefault = false;
            }

            // Tìm ảnh mới mà người dùng chọn để làm đại diện
            var selectedImage = db.productImages.FirstOrDefault(pi => pi.ProductImgID == id);
            if (selectedImage != null)
            {
                selectedImage.IsDefault = true;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                return Json(new { success = true });
            }

            // Trường hợp không tìm thấy ảnh với id được cung cấp
            return Json(new { success = false, message = "Không tìm thấy ảnh." });
        }

    }
}