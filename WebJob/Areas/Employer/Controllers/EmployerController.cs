using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace WebJob.Areas.Employer.Controllers
{
    public class EmployerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employer/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Employer/VerificationDetails
        [HttpGet]
        public ActionResult VerificationDetails()
        {
            var userId = User.Identity.GetUserId();
            var verification = db.employerVerifications
                                 .FirstOrDefault(v => v.AccountId == userId);

            if (verification == null)
            {
                ViewBag.Status = "Chưa xác thực";
                return View();
            }

            ViewBag.Status = verification.IsVerified ? "Đã xác thực" : "Đang chờ xác thực";
            return View(verification);
        }


        // GET: Employer/VerifyEmployerForm (Partial View)
        [HttpGet]
        public PartialViewResult VerifyEmployerForm()
        {
            return PartialView("_VerifyEmployerPartial", new EmployerVerificationViewModel());
        }

        // POST: Employer/VerifyEmployer
        // POST: Employer/VerifyEmployer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyEmployer(EmployerVerificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu chưa
                    bool emailExists = db.employerVerifications.Any(e => e.Email == model.Email);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email này đã được sử dụng. Vui lòng chọn email khác.");
                        return PartialView("_VerifyEmployerPartial", model);
                    }

                    if (model.VerificationDocument != null && model.VerificationDocument.ContentLength > 0)
                    {
                        // Kiểm tra định dạng và kích thước file
                        string fileExtension = Path.GetExtension(model.VerificationDocument.FileName)?.ToLower();
                        string[] allowedExtensions = { ".pdf", ".jpg", ".png" };

                        if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
                        {
                            ModelState.AddModelError("VerificationDocument", "Chỉ chấp nhận các định dạng file: PDF, JPG, PNG.");
                            return PartialView("_VerifyEmployerPartial", model);
                        }

                        if (model.VerificationDocument.ContentLength > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("VerificationDocument", "Kích thước file không được vượt quá 5 MB.");
                            return PartialView("_VerifyEmployerPartial", model);
                        }

                        // Lưu file vào thư mục
                        string uploadDir = Server.MapPath("~/Uploads/VerificationDocuments");
                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        // Tạo tên file duy nhất
                        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        string filePath = Path.Combine(uploadDir, uniqueFileName);

                        // Lưu file vào thư mục
                        model.VerificationDocument.SaveAs(filePath);

                        // Lưu đường dẫn tương đối của file vào cơ sở dữ liệu
                        string relativeFilePath = "~/Uploads/VerificationDocuments/" + uniqueFileName;

                        // Lưu thông tin vào cơ sở dữ liệu
                        db.employerVerifications.Add(new EmployerVerification
                        {
                            CompanyName = model.CompanyName,
                            PhoneNumber = model.PhoneNumber,
                            Email = model.Email,
                            BusinessLicenseNumber = model.BusinessLicenseNumber,
                            VerificationDocumentPath = relativeFilePath,  // Lưu đường dẫn tương đối
                            AccountId = User.Identity.GetUserId(),
                            IsVerified = false,
                            CreatedDate = DateTime.Now
                        });
                        db.SaveChanges();

                        // Xác nhận gửi thông tin thành công
                        TempData["SuccessMessage"] = "Thông tin xác thực đã được gửi. Chúng tôi sẽ xử lý trong thời gian sớm nhất.";
                        return RedirectToAction("Confirmation");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại.");
                }
            }

            return PartialView("_VerifyEmployerPartial", model);
        }


        // GET: Employer/Confirmation
        public ActionResult Confirmation()
        {
            return View();
        }
    }
}
