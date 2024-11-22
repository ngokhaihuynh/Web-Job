using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_JobApplication")]
    public class JobApplication : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobApplicationID { get; set; }

        public int JobID { get; set; }
        public int ApplicantID { get; set; }
        public string CoverLetter { get; set; }
        public string Note { get; set; }

       

        public virtual Job Job { get; set; }
        public virtual Applicant Applicant { get; set; }
        
    }

}