using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Order")]
    public class Order
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
        public int TypePayment  { get; set; } // 1 là thanh toán cod, 2 là thanh toán chuyển khoản
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; } // 1 là chưa thanh toán, 2 là đã thanh toán
        public string UserId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();


    }
}