using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebJob.Models.EF
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsID { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        public string Alias { get; set; }

       
        [AllowHtml]
        public string Content { get; set; }

        [StringLength(300)]
        public string ImageURL { get; set; }

        public bool IsActive { get; set; }
    }

}