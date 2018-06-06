using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class WorkerSkill
    {
        
        [Key, Column(Order=1)]
        [DisplayName("Worker")]
        public string WorkerID { get; set; }

        [Key, Column(Order =2)]
        [DisplayName("Skill")]
        public int SkillID { get; set; }

        [DisplayName("Years Of Experience")]
        public int YearsOfExperience { get; set; }

        public virtual Worker Worker { get; set; }

        public virtual Skill Skill { get; set; }

    }
}