using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Setting")]
    public class Setting:CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }

        
        [StringLength(255)]
        public string SettingKey { get; set; }

        
        [StringLength(255)]
        public string SettingValue { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

   
    }

}