using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebJob.Models.EF
{
    [Table("tb_Article")]
    public class Article : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Alias { get; set; }

        
        [AllowHtml]
        public string Content { get; set; }

        [StringLength(255)]
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
    }

}