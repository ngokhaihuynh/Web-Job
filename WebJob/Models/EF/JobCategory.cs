using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_JobCategory")]
    public class JobCategory: CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCategoryID { get; set; }

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }
        
        [StringLength(255)]
        public string Alias { get; set; }
        [StringLength(255)]
        public string Icon { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }

}