using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
namespace WebJob.Models.EF
{
    public class EmployerVerification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string BusinessLicenseNumber { get; set; }

        public string VerificationDocumentPath { get; set; }

        public string AccountId { get; set; }  // Liên kết với ApplicationUser

        public bool IsVerified { get; set; } // trạng thái xác thực

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("AccountId")]
        public virtual ApplicationUser Account { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();

    }

}
