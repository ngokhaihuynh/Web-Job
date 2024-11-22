using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebJob.Models
{
    public class EmployerVerificationViewModel
    {
        [Required(ErrorMessage = "Tên công ty không được để trống.")]
        [Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại liên hệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email liên hệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số giấy phép kinh doanh không được để trống.")]
        [Display(Name = "Số giấy phép kinh doanh")]
        public string BusinessLicenseNumber { get; set; }

        [Display(Name = "Tài liệu xác minh (PDF, JPG, PNG)")]
        public HttpPostedFileBase VerificationDocument { get; set; }
    }

}