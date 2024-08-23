using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;
using PagedList;


namespace WebJob.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Company
        public ActionResult Index(int? page)
        {
            IEnumerable<Company> items = db.Companies.OrderByDescending(x => x.CompanyID);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            // Chuẩn bị dữ liệu cho dropdown nếu cần
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Company model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                // Khởi tạo danh sách hình ảnh nếu chưa có
                if (model.CompanyImages == null)
                {
                    model.CompanyImages = new List<CompanyImage>();
                }

                // Kiểm tra hình ảnh và danh sách hình ảnh mặc định
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        bool isDefault = rDefault != null && rDefault.Contains(i + 1);
                        model.CompanyImages.Add(new CompanyImage
                        {
                            // Sử dụng CompanyID nếu CompanyImage liên kết với Company
                            CompanyID = model.CompanyID,
                            ImageURL = Images[i],
                            IsDefault = isDefault
                        });
                    }
                }

                // Cập nhật thông tin ngày tạo và ngày chỉnh sửa
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                // Thêm thông tin công ty vào cơ sở dữ liệu
                db.Companies.Add(model);
                db.SaveChanges();

                // Chuyển hướng về trang Index
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, khôi phục dữ liệu và hiển thị lại trang
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.Companies.Find(id);
            if (item != null)
            {
                db.Companies.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }


    }
}