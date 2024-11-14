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
            if (id == null)
            {
                // Xử lý trường hợp không có ID
                return HttpNotFound("Job ID không hợp lệ.");
            }

            ViewBag.JobID = id;

            // Lấy danh sách ảnh dựa trên JobID
            var items = db.CompanyImages.Where(x => x.JobID == id).ToList();

            if (!items.Any())
            {
                // Nếu không có ảnh nào được tìm thấy
                ViewBag.CompanyID = 0; // Hoặc bạn có thể xử lý khác
            }
            else
            {
                // Nếu có ảnh, lấy CompanyID từ ảnh đầu tiên
                ViewBag.CompanyID = items.First().CompanyID;
            }

            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int companyImgId, string url, int companyId, int jobId)
        {
            // Kiểm tra xem CompanyID có tồn tại không
            var companyExists = db.Companies.Any(c => c.CompanyID == companyId);
            var jobExists = db.Jobs.Any(j => j.JobID == jobId);

            if (!companyExists)
            {
                return Json(new { success = false, message = "Company không tồn tại." });
            }

            if (!jobExists)
            {
                return Json(new { success = false, message = "Job không tồn tại." });
            }

            // Nếu cả hai đều tồn tại, thêm ảnh vào cơ sở dữ liệu
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




        //[HttpPost]
        //public ActionResult AddImage(int companyImgId, string url, int companyId, int jobId)
        //{

        //    db.CompanyImages.Add(new CompanyImage
        //    {
        //        CompanyImageID = companyImgId,
        //        CompanyID = companyId,
        //        JobID = jobId,
        //        ImageURL = url,
        //        IsDefault = false
        //    });
        //    db.SaveChanges();
        //    return Json(new { success = true });
        //}


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.CompanyImages.Find(id);
            db.CompanyImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult SetDefaultImage(int id, int jobId)
        {
            // Tìm ảnh hiện tại đang là ảnh đại diện cho công việc cụ thể
            var currentDefaultImage = db.CompanyImages.FirstOrDefault(pi => pi.IsDefault && pi.JobID == jobId);

            // Nếu có ảnh đang là đại diện cho công việc, hủy chọn nó
            if (currentDefaultImage != null)
            {
                currentDefaultImage.IsDefault = false;
            }

            // Tìm ảnh mới mà người dùng chọn để làm đại diện
            var selectedImage = db.CompanyImages.FirstOrDefault(pi => pi.CompanyImageID == id && pi.JobID == jobId);
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