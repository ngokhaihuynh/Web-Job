using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_CompanyImage")]
    public class CompanyImage : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyImageID { get; set; }

        public int CompanyID { get; set; }
        public int JobID { get; set; }

       
        [StringLength(255)]
        public string ImageURL { get; set; }
        public bool IsDefault { get; set; }

        public virtual Company Company { get; set; }
        public virtual Job Job { get; set; }

    }

}