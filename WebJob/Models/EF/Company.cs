using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Company")]
    public class Company : CommonAbstract
    {

       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }

      
        [StringLength(255)]
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyAddress { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
        public virtual ICollection<CompanyImage> CompanyImages { get; set; } = new HashSet<CompanyImage>();
    }
}

