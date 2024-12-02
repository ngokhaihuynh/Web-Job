using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index(int? id, int? page)
        {
            // Bắt đầu với danh sách công việc đang hoạt động
            var query = db.Jobs.Where(x => x.IsActive);

            // Nếu có id hợp lệ, lọc theo danh mục công việc
            if (id > 0)
            {
                query = query.Where(x => x.JobCategoryID == id);
            }

            // Kiểm tra nếu không có công việc nào trong query
            if (!query.Any())
            {
                ViewBag.NoJobs = "Không có công việc nào được lưu bởi tài khoản này.";
            }

            // Thiết lập phân trang
            var pageNumber = page ?? 1; // Trang hiện tại
            var pageSize = 5;           // Số mục trên mỗi trang
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            // Chuyển danh sách query thành dạng phân trang
            var pagedList = query.OrderBy(x => x.JobID).ToPagedList(pageNumber, pageSize);

            // Cung cấp thông tin danh mục cho view
            ViewBag.CateId = id;

            return View(pagedList);
        }



        public ActionResult Detail(string alias, int id)
        {
            var item = db.Jobs.Find(id);

            // tang luot xem khi click vao
            if(item != null)
            {
                db.Jobs.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }

           

            // lấy công việc tương tự
            return View(item);
        }

        public ActionResult JobCategory(string alias, int id)
        {
            // Bắt đầu với danh sách công việc đang hoạt động
            var query = db.Jobs.Where(x => x.IsActive);

            // Nếu có id hợp lệ, lọc theo danh mục công việc
            if (id > 0)
            {
                query = query.Where(x => x.JobCategoryID == id);
            }

            // Chuyển đổi query thành danh sách
            var items = query.ToList();

            // Cung cấp thông tin danh mục cho view
            ViewBag.CateId = id;
            ViewBag.JobCount = items.Count;
            return View(items);
        }


        public ActionResult View_itemsByCateId(string alias, int? id)
        {
            // Tìm kiếm danh mục công việc dựa trên alias
            var category = db.JobCategories.FirstOrDefault(c => c.Alias == alias);
            if (category == null)
            {
                return HttpNotFound(); 
            }

            // Nếu id không phải null và khớp với danh mục công việc, tìm kiếm các công việc
            if (id.HasValue && id.Value == category.JobCategoryID && category.IsActive)
            {
                var items = db.Jobs.Where(x => x.JobCategoryID == category.JobCategoryID && x.IsActive).ToList();
                ViewBag.JobCount = items.Count;
                return View(items); 
            }

            return HttpNotFound(); 
        }


        // load Việc tuyển gấp
        /*  public ActionResult Partial_JobNow()
          {
              var items = db.Jobs
                  .Where(x => x.IsActive && x.IsNow)
                  .GroupBy(x => x.Company.CompanyName) // Nhóm theo tên công ty
                  .Select(g => g.FirstOrDefault())    // Lấy công việc đầu tiên trong mỗi nhóm
                  .Take(7)                           // Lấy tối đa 10 công việc
                  .ToList();

              return PartialView(items);
          }
  */
        public ActionResult Partial_JobNow()
        {
            var items = db.Jobs
                .Where(x => x.IsActive && x.IsNow)
                .GroupBy(x => x.Company.CompanyName) // Nhóm theo tên công ty
                .Select(g => g.OrderBy(_ => Guid.NewGuid()).FirstOrDefault()) // Lấy công việc ngẫu nhiên trong nhóm
                .OrderBy(_ => Guid.NewGuid()) // Sắp xếp ngẫu nhiên các nhóm
                .Take(7) // Lấy tối đa 7 công việc
                .ToList();

            return PartialView(items);
        }


        // load việc làm mới nhất
        public ActionResult Partial_JobLatest()
        {
            // Lấy danh sách 10 công việc mới nhất dựa trên ngày tạo (CreatedDate)
            var items = db.Jobs
                          .Where(x => x.IsActive)
                          .OrderByDescending(x => x.CreatedDate) // Sắp xếp theo ngày tạo mới nhất
                          .Take(10) // Giới hạn 10 công việc
                          .ToList();

            return PartialView(items); // Trả về PartialView với danh sách công việc
        }
    /*    public ActionResult Partial_CompanyLogos()
        {
            var companies = db.Companies
                              .GroupBy(c => c.CompanyName) // Nhóm theo tên công ty
                              .Select(g => g.FirstOrDefault())// Lấy công ty đầu tiên trong nhóm
                              .Take(8)
                              .ToList();
            return PartialView(companies);
        }*/

        public ActionResult Partial_CompanyLogos()
        {
            var companies = db.CompanyImages
                              .Where(x => x.IsDefault)
                              .GroupBy(c => c.Company.CompanyName) // Nhóm theo tên công ty
                              .Select(g => g.FirstOrDefault())// Lấy công ty đầu tiên trong nhóm
                              .Take(8)
                              .ToList();
            return PartialView(companies);
        }


        /* public ActionResult Partial_CompanyLogos()
         {
             // Lấy danh sách các ảnh theo công ty
             var companies = db.CompanyImages
                                   .Where(img => img.IsDefault) // Chỉ lấy ảnh mặc định
                                   .GroupBy(img => img.Company.CompanyName) // Nhóm theo tên công ty
                                   .Select(g => g.FirstOrDefault()) // Chọn ảnh đầu tiên trong mỗi nhóm
                                   .Take(8) // Giới hạn 8 kết quả
                                   .ToList();


             return PartialView(companies);
         }*/


        public ActionResult Partial_HighSalaryJobs()
        {
            // Lọc các công việc có mức lương > 10 triệu (10,000,000)
            var highSalaryJobs = db.Jobs.Where(x =>x.Salary.SalaryMax > 10000000 && x.IsActive).ToList();

            return PartialView(highSalaryJobs);
        }
        public PartialViewResult RelatedJobs(string categoryName, int excludeJobId)
        {
            // Lấy danh sách việc làm tương tự từ database
            var relatedJobs = db.Jobs
                                .Where(x => x.JobCategory.CategoryName == categoryName && x.JobID != excludeJobId)
                                .OrderByDescending(x => x.CreatedDate)
                                .Take(5) // Lấy 5 việc làm mới nhất
                                .ToList();

            return PartialView("_RelatedJobs", relatedJobs);
        }


    }
}