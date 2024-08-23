using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Position")]
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionID { get; set; }

    
        [StringLength(255)]
        public string PositionName { get; set; }

        public bool IsActive { get; set; } = true;
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}

