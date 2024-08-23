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
        public string CustomerName {get; set;}
        public string Phone { get; set; }
        public string Address { get; set; }
        public string TotalAmount { get; set; }
        public int Quantyti  { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();


    }
}