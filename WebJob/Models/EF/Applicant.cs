﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Applicant")]
    public class Applicant : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicantID { get; set; }


        [StringLength(255)]
        public string FullName { get; set; }


        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }


        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string CVFilePath { get; set; }

        public string CoverLetter { get; set; }

        public int ViewStatus { get; set; } = 0; // 0 = chưa xem, 1 = đã xem
                                                 // Liên kết với tài khoản người dùng
        [StringLength(128)] // Độ dài của UserId theo Identity
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


        // Thêm trường phản hồi
        [StringLength(1000)]
        public string FeebBack { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    }

}