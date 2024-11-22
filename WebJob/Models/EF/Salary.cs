using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WebJob.Models.EF
{
    [Table("tb_Salary")]
    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryID { get; set; }

        
        [StringLength(50)]
        public string SalaryRange { get; set; }
        
        public int SalaryMin { get; set; }

        public int SalaryMax { get; set; }


        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }

}