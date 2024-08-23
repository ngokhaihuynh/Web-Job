using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_ProductImage")]

    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductImgID { get; set; }
        public int ProductID { get; set; }
        public string Image { get; set; }
        public bool IsDefault { get; set; }
        public virtual Product Product { get; set; }

    }
}