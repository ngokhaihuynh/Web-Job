using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("tb_JobSkill")]
    public class JobSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobSkillID { get; set; }

        public int JobID { get; set; }
        public int SkillID { get; set; }

        public virtual Job Job { get; set; }
        public virtual Skill Skill { get; set; }
    }

}