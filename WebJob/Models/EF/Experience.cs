using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Experience")]
    public class Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExperienceID { get; set; }

        [StringLength(255)]
        public string ExperienceLevel { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }

}