using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;

namespace WebJob.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search

        public ActionResult Index(string keyword, string category, string location, string salaryRange, string experience, string skill)
        {
            // Kiểm tra nếu có tham số tìm kiếm, chuyển hướng sang SearchController
            if (!string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(category) || !string.IsNullOrEmpty(location) ||
                !string.IsNullOrEmpty(salaryRange) || !string.IsNullOrEmpty(experience) || !string.IsNullOrEmpty(skill))
            {
                return RedirectToAction("Search", "Search", new
                {   
                    keyword,
                    category,
                    location,
                    salaryRange,
                    experience,
                    skill
                });
            }

            // Hiển thị trang chủ nếu không có tham số tìm kiếm
            return View();
        }


        public ActionResult SearchView()
        {
            // Truy vấn dữ liệu từ cơ sở dữ liệu, loại bỏ trùng lặp
            ViewBag.Categories = db.JobCategories
                                   .Select(c => c.CategoryName)
                                   .Distinct()
                                   .ToList();

            ViewBag.Locations = db.Locations
                                  .Select(l => l.LocationName)
                                  .Distinct()
                                  .ToList();

            ViewBag.Skills = db.JobSkills
                               .Select(s => s.JobSkillName)
                               .Distinct()
                               .ToList();

            ViewBag.Experiences = db.Experiences
                                    .Select(e => e.ExperienceLevel)
                                    .Distinct()
                                    .ToList();

            ViewBag.Salaries = db.Salaries
                                 .Select(s => s.SalaryRange)
                                 .Distinct()
                                 .ToList();

            return PartialView("_SearchFormPartial");
        }




        public ActionResult Search(string keyword, string category, string location, string salaryRange, string experience, string skill, string companyName)
        {
            // Lấy tất cả công việc từ cơ sở dữ liệu
            var jobs = db.Jobs.Where(j => j.IsActive).AsQueryable();

            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(keyword) || j.Company.CompanyName.Contains(keyword));
            }

            // Lọc theo ngành nghề
            if (!string.IsNullOrEmpty(category))
            {
                jobs = jobs.Where(j => j.JobCategory.CategoryName == category);
            }

            // Lọc theo địa điểm
            if (!string.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j => j.Location.LocationName == location);
            }

            // Lọc theo mức lương
            // Lọc theo mức lương
            if (!string.IsNullOrEmpty(salaryRange))
            {
                var salaryParts = salaryRange.Split('-').Select(s => s.Trim()).ToArray();
                if (salaryParts.Length == 2 &&
                    int.TryParse(salaryParts[0], out int minSalary) &&
                    int.TryParse(salaryParts[1], out int maxSalary))
                {
                    jobs = jobs.Where(j =>
                        j.Salary.SalaryMin <= maxSalary && j.Salary.SalaryMax >= minSalary);
                }
                else
                {
                    // Log lỗi khi không thể parse mức lương
                    Console.WriteLine($"Invalid salary range format: {salaryRange}");
                }
            }


            // Lọc theo kinh nghiệm
            if (!string.IsNullOrEmpty(experience))
            {
                jobs = jobs.Where(j => j.Experience.ExperienceLevel == experience);
            }

            // Lọc theo kỹ năng
            if (!string.IsNullOrEmpty(skill))
            {
                jobs = jobs.Where(j => j.JobSkill.JobSkillName.Contains(skill));
            }

            // Lọc theo tên công ty
            if (!string.IsNullOrEmpty(companyName))
            {
                jobs = jobs.Where(j => j.Company.CompanyName.Contains(companyName));
            }

            // Trả về danh sách công việc
            return View("SearchResults", jobs.ToList());
        }



    }
}