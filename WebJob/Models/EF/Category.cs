using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Category")]
    public class Category : CommonAbstract
    {
   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Tên danh mục không được để trống")]
        [StringLength(150)]
        public string Title { get; set; }
        public CategoryType CategoryType { get; set; }
        public string Alias { get; set; }

        //[StringLength(150)]
        //public string TypeCode { get; set; }
        //public string Link { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
        
        public int Position { get; set; }
        //public int CategoryType { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public bool IsActive { get; set; }
       
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
    public enum CategoryType
    {
        Candidate = 1,  // Danh mục dành cho ứng viên
        Employer = 2,    // Danh mục dành cho nhà tuyển dụng
        Both = 3 // cho cả 2
    }

}