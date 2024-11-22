using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;


namespace WebJob.Controllers
{
    public class MyApplicantController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MyApplicant
        public ActionResult Index()
        {
            return View();
        }
        // GET: MyApplicant
        public ActionResult MyAppliedJobs(int? page)
        {
            // Lấy ID của người dùng đang đăng nhập
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Nếu người dùng chưa đăng nhập, chuyển hướng tới trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách công việc mà người dùng đã ứng tuyển
            var appliedJobs = db.JobApplications
                                .Where(ja => ja.Applicant.UserId == userId)  // Lọc theo UserId
                                .Select(ja => ja.Job)  // Lấy thông tin công việc
                                .OrderByDescending(j => j.CreatedDate)  // Sắp xếp theo ngày đăng
                                .ToList();

            // Phân trang danh sách công việc đã ứng tuyển
            var pageNumber = page ?? 1;
            var pageSize = 10; // Số lượng công việc mỗi trang
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            return View(appliedJobs.ToPagedList(pageNumber, pageSize));
        }



    }
}