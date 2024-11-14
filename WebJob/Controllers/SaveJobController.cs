using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Controllers
{
    public class SaveJobController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: SaveJob
        public ActionResult Index()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if(save != null)
            {
                return View(save.Items);
            }
            return View();
        }

        // Trang ứng tuyển
        [HttpGet]
        public ActionResult AppllyJob(int jobId)
        {
            // Tạo model để gửi vào view
            var model = new ApplicantViewModel
            {
                JobID = jobId
            };

            return View(model);
        }


        // Xử lý ứng tuyển
        // Action xử lý form khi người dùng gửi đơn ứng tuyển
        //[HttpPost]
        //public ActionResult AppllyJob(ApplicantViewModel model, int jobId, HttpPostedFileBase CVFilePath)
        //{
        //    // Kiểm tra nếu ModelState hợp lệ và có file đính kèm
        //    if (ModelState.IsValid && CVFilePath != null)
        //    {
        //        // Kiểm tra định dạng file
        //        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".docx" };
        //        var fileExtension = Path.GetExtension(CVFilePath.FileName).ToLower();
        //        if (!allowedExtensions.Contains(fileExtension))
        //        {
        //            return Json(new { success = false, message = "Chỉ chấp nhận file PDF, JPG, PNG, hoặc DOCX." });
        //        }

        //        // Kiểm tra kích thước file (giới hạn 10 MB)
        //        const int maxFileSize = 10 * 1024 * 1024; // 10 MB
        //        if (CVFilePath.ContentLength > maxFileSize)
        //        {
        //            return Json(new { success = false, message = "Kích thước file không được vượt quá 10 MB." });
        //        }

        //        // Tạo tên file và đường dẫn để lưu file
        //        string fileName = Path.GetFileName(CVFilePath.FileName);
        //        string filePath = Path.Combine(Server.MapPath("~/Uploads/files"), fileName);

        //        try
        //        {
        //            // Lưu file vào thư mục
        //            CVFilePath.SaveAs(filePath);

        //            // Tạo và lưu đối tượng Applicant
        //            var applicant = new Applicant
        //            {
        //                FullName = model.FullName,
        //                PhoneNumber = model.PhoneNumber,
        //                Email = model.Email,
        //                CoverLetter = model.CoverLetter,
        //                CVFilePath = "/Uploads/files/" + fileName, // Đường dẫn tương đối
        //                CreatedBy = model.FullName,
        //                CreatedDate = DateTime.Now,
        //                ModifiedDate = DateTime.Now
        //            };
        //            db.Applicants.Add(applicant);
        //            db.SaveChanges();

        //            // Tạo và lưu đối tượng JobApplication
        //            var jobApplication = new JobApplication
        //            {
        //                JobID = jobId,
        //                ApplicantID = applicant.ApplicantID,
        //                CoverLetter = model.CoverLetter,
        //                CreatedDate = DateTime.Now,
        //                ModifiedDate = DateTime.Now
        //            };

        //            db.JobApplications.Add(jobApplication);
        //            db.SaveChanges();

        //            return Json(new { success = true, message = "Ứng tuyển thành công!", redirectUrl = Url.Action("Index", "viec-lam") });

        //        }
        //        catch (Exception ex)
        //        {
        //            // Xử lý lỗi lưu file hoặc lưu vào cơ sở dữ liệu
        //            return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình ứng tuyển. Vui lòng thử lại sau." });
        //        }
        //    }

        //    // Trường hợp dữ liệu không hợp lệ hoặc thiếu file
        //    return Json(new { success = false, message = "Thông tin ứng tuyển không hợp lệ hoặc không có file đính kèm." });
        //}


        [HttpPost]
        public ActionResult AppllyJob(ApplicantViewModel model, int jobId, HttpPostedFileBase CVFilePath)
        {
            // Kiểm tra nếu ModelState hợp lệ và có file đính kèm
            if (ModelState.IsValid && CVFilePath != null)
            {
                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".docx" };
                var fileExtension = Path.GetExtension(CVFilePath.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Json(new { success = false, message = "Chỉ chấp nhận file PDF, JPG, PNG, hoặc DOCX." });
                }

                // Kiểm tra kích thước file (giới hạn 10 MB)
                const int maxFileSize = 10 * 1024 * 1024; // 10 MB
                if (CVFilePath.ContentLength > maxFileSize)
                {
                    return Json(new { success = false, message = "Kích thước file không được vượt quá 10 MB." });
                }

                // Tạo tên file và đường dẫn để lưu file
                string fileName = Path.GetFileName(CVFilePath.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Uploads/files"), fileName);

                try
                {
                    // Lưu file vào thư mục
                    CVFilePath.SaveAs(filePath);

                    // Tạo và lưu đối tượng Applicant
                    var applicant = new Applicant
                    {
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        CoverLetter = model.CoverLetter,
                        CVFilePath = "/Uploads/files/" + fileName, // Đường dẫn tương đối
                        CreatedBy = model.FullName,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    db.Applicants.Add(applicant);
                    db.SaveChanges();

                    // Tạo và lưu đối tượng JobApplication
                    var jobApplication = new JobApplication
                    {
                        JobID = jobId,
                        ApplicantID = applicant.ApplicantID,
                        CoverLetter = model.CoverLetter,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    db.JobApplications.Add(jobApplication);
                    db.SaveChanges();

                    // Lấy email nhà tuyển dụng từ bảng Jobs và Company
                    //var job = db.Jobs.Include(j => j.Company).FirstOrDefault(j => j.JobID == jobId);
                    var job = db.Jobs.Include("Company").FirstOrDefault(j => j.JobID == jobId);
                    var recruiterEmail = job?.Company?.CompanyEmail;
                    string jobname = job.JobTitle;
                    if (string.IsNullOrEmpty(recruiterEmail))
                    {
                        return Json(new { success = false, message = "Không tìm thấy email nhà tuyển dụng." });
                    }

                    // Đọc nội dung file HTML và thay thế các chỗ placeholder
                    string emailContent = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send3.html"));
                    emailContent = emailContent.Replace("{{FullName}}", model.FullName)
                                               .Replace("{{PhoneNumber}}", model.PhoneNumber)
                                               .Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"))
                                               .Replace("{{CoverLetter}}", model.CoverLetter)
                                               .Replace("{{CVLink}}", "/Uploads/files/" + fileName)
                                               .Replace("{{CVFileName}}", fileName)
                                               .Replace("{{JobName}}", jobname);

                    // Gửi email cho nhà tuyển dụng
                    bool mailSent = WebJob.Models.Common.common.SendMail("Thông Tin Ứng Viên", "Ứng tuyển vị trí Lập Trình Viên", emailContent, recruiterEmail);

                    if (mailSent)
                    {
                        return Json(new { success = true, message = "Ứng tuyển thành công và email đã được gửi!" , redirectUrl = Url.Action("Index", "viec-lam") });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Đã xảy ra lỗi khi gửi email. Vui lòng thử lại sau." });
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi lưu file hoặc lưu vào cơ sở dữ liệu
                    return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình ứng tuyển. Vui lòng thử lại sau." });
                }
            }

            // Trường hợp dữ liệu không hợp lệ hoặc thiếu file
            return Json(new { success = false, message = "Thông tin ứng tuyển không hợp lệ hoặc không có file đính kèm." });
        }

        public ActionResult ShowCount()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if(save != null)
            {
                return Json(new { Count = save.Items.Count }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { Count = 0}, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult AddSaveJob(int id)
        {
            var code = new { success = false, msg="", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkJob = db.Jobs.FirstOrDefault(x => x.JobID == id);
            if(checkJob != null)
            {
                SaveJob save = (SaveJob)Session["Save"];
                if(save == null)
                {
                    save = new SaveJob();
                }
                SaveJobItem item = new SaveJobItem
                {
                    SaveJobId = checkJob.JobID,
                    SaveJobName = checkJob.JobTitle,
                    SaveJobCate = checkJob.JobCategory.CategoryName,
                    Alias = checkJob.Alias,
                    SaveJobSalaryMin = checkJob.Salary.SalaryMin,
                    SaveJobSalaryMax = checkJob.Salary.SalaryMax

                };
                if (checkJob.CompanyImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.SaveJobImg = checkJob.CompanyImages.FirstOrDefault(x => x.IsDefault).ImageURL;
                }

                save.AddSaveJob(item);
                Session["Save"] = save;
                code = new { success = true, msg = "Lưu thành công", code = 1, Count = save.Items.Count };

            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { success = false, msg = "", code = -1, Count = 0 };
            SaveJob save = (SaveJob)Session["Save"];
            if (save != null)
            {
                var checkJob = save.Items.FirstOrDefault(x => x.SaveJobId == id);
                if(checkJob != null)
                {
                    save.Remove(id);
                    code = new { success = true, msg = "", code = 1, Count = save.Items.Count };
                }
            }
            return Json(code);
        }


        public ActionResult GetSaveJobItems()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if (save != null)
            {
                return PartialView("_SaveJobView", save.Items);
            }
            return PartialView("_SaveJobView", new List<SaveJobItem>()); // trả về danh sách rỗng nếu không có dữ liệu
        }

        [HttpPost]
        public ActionResult DeleteAllJobs()
        {
            SaveJob save = (SaveJob)Session["Save"];
            if (save != null)
            {
                save.ClearSave();
                return Json(new { success = true, message = "Đã xóa tất cả công việc đã lưu" });
            }
            return Json(new { success = false, message = "Có lỗi xảy ra: " });
        }

    }
}