using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class WorkerSkill
    {
        [Key]
        public int WSID { get; set; }

        public int WorkerID { get; set; }

        public int SkillID { get; set; }

        public virtual Worker Worker { get; set; }

        public virtual Skill Skill { get; set; }

    }
}