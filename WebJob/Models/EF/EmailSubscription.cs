using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_EmailSubscription")]
    public class EmailSubscription : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriptionID { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        public int? JobCategoryID { get; set; }
        public int? LocationID { get; set; }
        public int? PositionID { get; set; }
        public string SalaryRange { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual JobCategory JobCategory { get; set; }
        public virtual Location Location { get; set; }
        public virtual Position Position { get; set; }
    }

}