using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_CategoryProduct")]
    public class CategoryProduct : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CateProId { get; set; }
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(250)]
        public string Title { get; set; }

        public string Alias { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}