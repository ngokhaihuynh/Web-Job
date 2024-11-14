using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Order")]
    public class Order:CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage ="Không được để trống!")]
        public string CustomerName {get; set;}
        [Required(ErrorMessage = "Không được để trống!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Không được để trống!")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "Không được để trống!")]
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity  { get; set; }
        public int TypePayment  { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();


    }
}