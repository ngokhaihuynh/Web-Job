using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Location")]
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationID { get; set; }

        [Required]
        [StringLength(255)]
        public string LocationName { get; set; }

        public bool IsActive { get; set; } = true;
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }

}