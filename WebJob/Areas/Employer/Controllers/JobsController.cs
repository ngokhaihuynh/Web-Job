using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Employer.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Jobs
        public ActionResult Index(int? page)
        {
            // Lấy ID người dùng đang đăng nhập
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Chuyển hướng người dùng đến trang đăng nhập nếu chưa đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lọc danh sách công việc theo UserId
            IEnumerable<Job> items = db.Jobs
                .Where(x => x.UserId == userId) // Lọc theo UserId của tài khoản đăng nhập
                .OrderByDescending(x => x.JobID);

            // Thiết lập phân trang
            //var pageSize = 10;
            //var pageIndex = page ?? 1; // Nếu page là null, gán mặc định là 1

            // Kiểm tra xem có công việc nào hay không
            if (!items.Any())
            {
                ViewBag.NoJobs = "Không có công việc nào được lưu bởi tài khoản này.";
            }

            var pageNumber = page ?? 1;
            var pageSize = 8;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));


            /*// Phân trang
            items = items.ToPagedList(pageNumber, pageSize);


            // Truyền dữ liệu vào View
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items);*/
        }


        public ActionResult Add()
        {
            // Kiểm tra và thêm dữ liệu mặc định vào bảng Experience nếu chưa có
            if (!db.Experiences.Any())
            {
                var experienceLevels = new List<Experience>
        {
            new Experience { ExperienceLevel = "Chưa có kinh nghiệm" },
            new Experience { ExperienceLevel = "Dưới 1 năm" },
            new Experience { ExperienceLevel = "2 năm" },
            new Experience { ExperienceLevel = "3 năm" },
            new Experience { ExperienceLevel = "4-5 năm" },
            new Experience { ExperienceLevel = "Trên 5 năm" }
        };

                db.Experiences.AddRange(experienceLevels);
                db.SaveChanges();
            }
            //
            if (!db.Salaries.Any())
            {
                var salaryRanges = new List<Salary>
                {
                    new Salary { SalaryRange = "Dưới 10 triệu", SalaryMin = 0, SalaryMax = 9999999 },
                    new Salary { SalaryRange = "Từ 10-15 triệu", SalaryMin = 10000000, SalaryMax = 15000000 },
                    new Salary { SalaryRange = "Từ 15-20 triệu", SalaryMin = 15000000, SalaryMax = 20000000 },
                    new Salary { SalaryRange = "Từ 20-25 triệu", SalaryMin = 20000000, SalaryMax = 25000000 },
                    new Salary { SalaryRange = "Từ 25-30 triệu", SalaryMin = 25000000, SalaryMax = 30000000 },
                    new Salary { SalaryRange = "Từ 30-50 triệu", SalaryMin = 30000000, SalaryMax = 50000000 },
                    new Salary { SalaryRange = "Trên 50 triệu", SalaryMin = 50000000, SalaryMax = int.MaxValue }
                };
                db.Salaries.AddRange(salaryRanges);
                db.SaveChanges();
            }

            // Lấy danh sách các JobCategory và Experience để hiển thị
            ViewBag.Salary = new SelectList(db.Salaries.ToList(), "SalaryID", "SalaryRange");
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
            ViewBag.Experience = new SelectList(db.Experiences.ToList(), "ExperienceID", "ExperienceLevel");

            return View();
        }
/*
        public ActionResult Add()
        {
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");    
            return View();
        }*/


        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Add(Job model, List<string> Images, List<int> rDefault)
         {
             // Kiểm tra người dùng đã đăng nhập hay chưa
             var userId = User.Identity.GetUserId();
             if (string.IsNullOrEmpty(userId))
             {
                 ModelState.AddModelError("", "Bạn cần đăng nhập trước khi thêm công việc.");
                 return RedirectToAction("Login", "Account"); // Chuyển hướng người dùng đến trang đăng nhập nếu chưa đăng nhập
             }

             // Lấy thông tin tài khoản đăng nhập từ bảng EmployerVerifications
             var employer = db.employerVerifications.FirstOrDefault(e => e.AccountId == userId);
             if (employer == null || !employer.IsVerified)
             {
                 // Nếu tài khoản chưa được xác thực, chuyển hướng đến trang xác thực
                 ModelState.AddModelError("", "Tài khoản của bạn chưa được xác thực. Vui lòng xác thực tài khoản trước khi đăng tin.");
                 return RedirectToAction("VerifyEmployerForm", "Employer");
             }

             // Nếu ModelState hợp lệ, tiếp tục xử lý công việc
             if (ModelState.IsValid)
             {
                 // Gắn EmployerVerificationId vào Job
                 model.EmployerVerificationId = employer.Id;

                 // Gán UserId vào Job (trường này không được phép NULL)
                 model.UserId = userId; // Gán UserId vào Job

                 // Xử lý hình ảnh nếu có
                 if (Images != null && Images.Count > 0)
                 {
                     for (int i = 0; i < Images.Count; i++)
                     {
                         model.CompanyImages.Add(new CompanyImage
                         {
                             ImageURL = Images[i],
                             IsDefault = rDefault != null && rDefault.Contains(i + 1) // Kiểm tra ảnh mặc định
                         });
                     }
                 }

                 // Gán thông tin mặc định cho Job
                 model.CreatedDate = DateTime.Now;
                 model.ModifiedDate = DateTime.Now;

                 // Tạo alias nếu chưa có
                 if (string.IsNullOrEmpty(model.Alias))
                 {
                     model.Alias = WebJob.Models.Common.Filter.FilterChar(model.JobTitle);
                 }

                 // Lưu công việc vào cơ sở dữ liệu
                 db.Jobs.Add(model);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             // Nếu không hợp lệ, trả về View và giữ lại dữ liệu đã nhập
             ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
             return View(model);
         }*/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Job model, List<string> Images, List<int> rDefault)
        {
            // Kiểm tra người dùng đã đăng nhập hay chưa
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Bạn cần đăng nhập trước khi thêm công việc.");
                return RedirectToAction("Login", "Account"); // Chuyển hướng người dùng đến trang đăng nhập nếu chưa đăng nhập
            }

            // Lấy thông tin tài khoản đăng nhập từ bảng EmployerVerifications
            var employer = db.employerVerifications.FirstOrDefault(e => e.AccountId == userId);
            if (employer == null || !employer.IsVerified)
            {
                ModelState.AddModelError("", "Tài khoản của bạn chưa được xác thực. Vui lòng xác thực tài khoản trước khi đăng tin.");
                return RedirectToAction("VerifyEmployerForm", "Employer");
            }

            // Kiểm tra và thêm dữ liệu mặc định vào bảng Experience
            if (!db.Experiences.Any())
            {
                var experienceLevels = new List<Experience>
        {
            new Experience { ExperienceLevel = "Chưa có kinh nghiệm" },
            new Experience { ExperienceLevel = "Dưới 1 năm" },
            new Experience { ExperienceLevel = "2 năm" },
            new Experience { ExperienceLevel = "3 năm" },
            new Experience { ExperienceLevel = "4-5 năm" },
            new Experience { ExperienceLevel = "Trên 5 năm" }
        };

                db.Experiences.AddRange(experienceLevels);
                db.SaveChanges();
            }
            // Tạo dữ liệu mặc định cho bảng Salary nếu chưa có
            if (!db.Salaries.Any())
            {
                var salaryRanges = new List<Salary>
                {
                    new Salary { SalaryRange = "Dưới 10 triệu", SalaryMin = 0, SalaryMax = 9999999 },
                    new Salary { SalaryRange = "Từ 10-15 triệu", SalaryMin = 10000000, SalaryMax = 15000000 },
                    new Salary { SalaryRange = "Từ 15-20 triệu", SalaryMin = 15000000, SalaryMax = 20000000 },
                    new Salary { SalaryRange = "Từ 20-25 triệu", SalaryMin = 20000000, SalaryMax = 25000000 },
                    new Salary { SalaryRange = "Từ 25-30 triệu", SalaryMin = 25000000, SalaryMax = 30000000 },
                    new Salary { SalaryRange = "Từ 30-50 triệu", SalaryMin = 30000000, SalaryMax = 50000000 },
                    new Salary { SalaryRange = "Trên 50 triệu", SalaryMin = 50000000, SalaryMax = int.MaxValue }
                };
                db.Salaries.AddRange(salaryRanges);
                db.SaveChanges();
            }

            // Nếu ModelState hợp lệ, tiếp tục xử lý công việc
            if (ModelState.IsValid)
            {
                // Gắn EmployerVerificationId vào Job
                model.EmployerVerificationId = employer.Id;

                // Gán UserId vào Job (trường này không được phép NULL)
                model.UserId = userId;

                // Xử lý hình ảnh nếu có
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        model.CompanyImages.Add(new CompanyImage
                        {
                            ImageURL = Images[i],
                            IsDefault = rDefault != null && rDefault.Contains(i + 1) // Kiểm tra ảnh mặc định
                        });
                    }
                }

                // Gán thông tin mặc định cho Job
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                // Tạo alias nếu chưa có
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = WebJob.Models.Common.Filter.FilterChar(model.JobTitle);
                }

                // Lưu công việc vào cơ sở dữ liệu
                db.Jobs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu không hợp lệ, trả về View và giữ lại dữ liệu đã nhập
            ViewBag.Salary = new SelectList(db.Salaries.ToList(), "SalaryID", "SalaryRange");
            ViewBag.Experience = new SelectList(db.Experiences.ToList(), "ExperienceID", "ExperienceLevel"); // Load danh sách kinh nghiệm
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName");
            return View(model);
        }



        /*public ActionResult Edit(int? id)
        {
            // Kiểm tra xem ID có hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy dữ liệu Job từ cơ sở dữ liệu
            var jobInDb = db.Jobs.Find(id);
            if (jobInDb == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách JobCategory cho SelectList


            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName", jobInDb.JobCategoryID);

            return View(jobInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Job model)
        {
            if (ModelState.IsValid)
            {
                // Tìm bản ghi Job trong cơ sở dữ liệu
                var jobInDb = db.Jobs.Find(model.JobID);
                if (jobInDb == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy bản ghi cần chỉnh sửa.");
                    return View(model);
                }

                try
                {
                    // Cập nhật các thuộc tính cần thiết, trừ CompanyID
                    jobInDb.JobTitle = model.JobTitle;
                    jobInDb.JobCategoryID = model.JobCategoryID;
                    jobInDb.ModifiedDate = DateTime.Now;
                    jobInDb.Alias = WebJob.Models.Common.Filter.FilterChar(model.JobTitle);
                    jobInDb.JobDescription = model.JobDescription;
                    jobInDb.IsActive = model.IsActive;
                    jobInDb.IsNow = model.IsNow;
                    jobInDb.Salary.SalaryRange = model.Salary.SalaryRange;
                    jobInDb.JobRequirements = model.JobRequirements;
                    jobInDb.Salary.SalaryMin = model.Salary.SalaryMin;
                    jobInDb.Salary.SalaryMax = model.Salary.SalaryMax;

                    // Kiểm tra và cập nhật Location và Experience nếu cần thiết
                    jobInDb.Experience.ExperienceLevel = model.Experience.ExperienceLevel;
                    jobInDb.Location.LocationName = model.Location.LocationName;
                    // Kiểm tra và cập nhật JobSkillName nếu có
                    if (model.JobSkill != null)
                    {
                        jobInDb.JobSkill.JobSkillName = model.JobSkill.JobSkillName;
                    }
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    ModelState.AddModelError("", "Có lỗi khi lưu thay đổi: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ, trả lại view với dữ liệu đã chỉnh sửa
        
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName", model.JobCategoryID);
            return View(model);
        }
*/
        public ActionResult Edit(int? id)
        {
            // Kiểm tra xem ID có hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy dữ liệu Job từ cơ sở dữ liệu
            var jobInDb = db.Jobs.Find(id);
            if (jobInDb == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách JobCategory cho SelectList

            ViewBag.Salary = new SelectList(db.Salaries.ToList(), "SalaryID", "SalaryRange", jobInDb?.SalaryID);
            ViewBag.Experience = new SelectList(db.Experiences.ToList(), "ExperienceID", "ExperienceLevel", jobInDb?.ExperienceID);
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName", jobInDb.JobCategoryID);

            return View(jobInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Job model)
        {
            if (ModelState.IsValid)
            {
                // Tìm bản ghi Job trong cơ sở dữ liệu
                var jobInDb = db.Jobs.Find(model.JobID);
                if (jobInDb == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy bản ghi cần chỉnh sửa.");
                    return View(model);
                }

                try
                {
                    // Cập nhật các thuộc tính cần thiết, trừ CompanyID
                    jobInDb.JobTitle = model.JobTitle;
                    jobInDb.JobCategoryID = model.JobCategoryID;
                    jobInDb.ModifiedDate = DateTime.Now;
                    jobInDb.Alias = WebJob.Models.Common.Filter.FilterChar(model.JobTitle);
                    jobInDb.JobBenefits = model.JobBenefits;
                    jobInDb.JobDescription = model.JobDescription;
                    jobInDb.IsActive = model.IsActive;
                    jobInDb.IsNow = model.IsNow;
                    jobInDb.EndDate = model.EndDate;
                    jobInDb.SalaryID = model.SalaryID;
                    jobInDb.ExperienceID = model.ExperienceID;
                    jobInDb.JobRequirements = model.JobRequirements;
                    /* jobInDb.Salary.SalaryRange = model.Salary.SalaryRange;
                     * jobInDb.Salary.SalaryMin = model.Salary.SalaryMin;
                     jobInDb.Salary.SalaryMax = model.Salary.SalaryMax;*/

                    // Kiểm tra và cập nhật Location và Experience nếu cần thiết
                   
                    jobInDb.Location.LocationName = model.Location.LocationName;
                    // Kiểm tra và cập nhật JobSkillName nếu có
                    if (model.JobSkill != null)
                    {
                        jobInDb.JobSkill.JobSkillName = model.JobSkill.JobSkillName;
                    }
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    ModelState.AddModelError("", "Có lỗi khi lưu thay đổi: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ, trả lại view với dữ liệu đã chỉnh sửa
            ViewBag.Salary = new SelectList(db.Salaries.ToList(), "SalaryID", "SalaryRange", model.SalaryID);
            ViewBag.Experience = new SelectList(db.Experiences.ToList(), "ExperienceID", "ExperienceLevel", model?.ExperienceID);
            ViewBag.JobCategory = new SelectList(db.JobCategories.ToList(), "JobCategoryID", "CategoryName", model.JobCategoryID);
            return View(model);
        }


    


        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.Jobs.Find(id);
            if (item != null)
            {
                db.Jobs.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult isActive(int id)
        {

            var item = db.Jobs.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });

            }
            return Json(new { success = false });
        }[HttpPost]
        public ActionResult IsNow(int id)
        {

            var item = db.Jobs.Find(id);
            if (item != null)
            {
                item.IsNow = !item.IsNow;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsNow = item.IsNow });

            }
            return Json(new { success = false });
        }

    }
}