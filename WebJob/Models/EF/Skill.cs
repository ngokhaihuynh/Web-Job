using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_Skill")]
    public class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillID { get; set; }

        
        [StringLength(255)]
        public string SkillName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public virtual ICollection<JobSkill> JobSkills { get; set; } = new HashSet<JobSkill>();
    }

}