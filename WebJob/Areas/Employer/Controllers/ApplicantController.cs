using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using PagedList;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace WebJob.Areas.Employer.Controllers
{
    public class ApplicantController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Employer/Applicant
        //public ActionResult Index(int? page)
        //{
        //    var items = db.Applicants.OrderByDescending(x => x.CreatedDate).ToList();

        //    if(page == null)
        //    {
        //        page = 1;
        //    }
        //    var pageNumber = page ?? 1;
        //    var pageSize = 15;
        //    return View(items.ToPagedList(pageNumber, pageSize));
        //}



        public ActionResult Index(int? page, int? jobId)
        {
            // Lấy ID người dùng hiện tại
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Nếu chưa đăng nhập, chuyển hướng người dùng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách công việc của người dùng hiện tại
            var jobs = db.Jobs.Where(j => j.UserId == userId).ToList();

            // Lấy danh sách JobID từ các công việc của người dùng
            var jobIds = jobs.Select(j => j.JobID).ToList();

            // Lấy danh sách Applicant cùng dữ liệu liên quan từ Job thông qua JobApplication
            var items = db.Applicants
                          .Include("JobApplications.Job")
                          .Where(a => a.JobApplications.Any(ja => jobIds.Contains(ja.JobID))) // So sánh JobID với danh sách JobIDs
                          .OrderByDescending(x => x.CreatedDate)
                          .ToList();

            // Nếu có JobId trong query string, lọc các ứng viên ứng tuyển vào công việc đó
            if (jobId.HasValue)
            {
                items = items
                    .Where(x => x.JobApplications.Any(ja => ja.JobID == jobId.Value))
                    .ToList();
            }

            // Lấy danh sách các công việc duy nhất từ JobApplications của các ứng viên
            var uniqueJobs = items
                .SelectMany(x => x.JobApplications)
                .Select(ja => ja.Job)
                .GroupBy(j => j.JobID)
                .Select(g => g.First())
                .Where(j => j.EndDate >= DateTime.Now) // Lọc công việc có ngày hết hạn chưa qua
                .ToList();

            // Truyền danh sách công việc duy nhất vào ViewBag
            ViewBag.UniqueJobs = uniqueJobs;

            // Phân trang cho danh sách ứng viên
            var pageNumber = page ?? 1;
            var pageSize = 8;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            // Trả về View với danh sách ứng viên đã được lọc và phân trang
            return View(items.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult ApplicantsByJob(int jobId, int? page)
        {
            // Lấy ID người dùng hiện tại
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Nếu chưa đăng nhập, chuyển hướng người dùng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy công việc theo JobID và UserId (nếu cần)
            var job = db.Jobs
                        .Include("JobApplications.Applicant")
                        .FirstOrDefault(j => j.JobID == jobId && j.UserId == userId); 

            if (job == null || job.EndDate < DateTime.Now) // Kiểm tra nếu công việc đã hết hạn hoặc không tồn tại
            {
                return HttpNotFound();
            }

            // Lấy danh sách ứng viên của công việc
            var applicants = job.JobApplications.Select(ja => ja.Applicant)
                                                .OrderByDescending(a => a.CreatedDate)
                                                .ToList();

            // Phân trang
            var pageNumber = page ?? 1;
            var pageSize = 8;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            
            ViewBag.UniqueJobs = db.Jobs.Where(j => j.UserId == userId && j.EndDate >= DateTime.Now).ToList();

            // Truyền thông tin công việc hiện tại vào ViewBag
            ViewBag.SelectedJobId = jobId;
            ViewBag.JobTitle = job.JobTitle; // Tên công việc

            return View(applicants.ToPagedList(pageNumber, pageSize));
        }


        // Action GET: Hiển thị thông tin ứng viên và nhận xét
        public ActionResult View(int id)
        {
            var applicant = db.Applicants.Find(id);

            if (applicant != null)
            {
                // Lọc công việc có ngày hết hạn chưa qua
                var jobs = db.Jobs.Include("JobApplications")
                                  .Where(j => j.EndDate >= DateTime.Now)
                                  .ToList();
                ViewBag.UniqueJobs = jobs;
                applicant.ViewStatus = 1; // Đánh dấu là đã xem
                db.SaveChanges();
            }

            return View(applicant);
        }

        // Cập nhật nhận xét 
        [HttpPost]
        public ActionResult View(int id, string feedback)
        {
            var applicant = db.Applicants.Find(id);

            if (applicant != null)
            {
                
                applicant.FeebBack = feedback;
                db.SaveChanges();
            }


            return RedirectToAction("View", new { id = id });
        }







    }
}