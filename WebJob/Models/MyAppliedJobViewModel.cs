using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebJob.Models
{
    public class MyAppliedJobViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FeebBack { get; set; } // Nhận xét từ bảng Applicant
    }

}