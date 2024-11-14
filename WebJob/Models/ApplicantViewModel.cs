using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebJob.Models
{
    public class ApplicantViewModel
    {

        public int JobID { get; set; }
        [StringLength(255)]
        public string FullName { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }


        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string CoverLetter { get; set; }

        [StringLength(500)]
        public string CVFilePath { get; set; }

    }
}