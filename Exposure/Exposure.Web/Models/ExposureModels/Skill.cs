using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class Skill
    {
        [ScaffoldColumn(false)]
        [DisplayName("Skill")]
        public int SkillID { get; set; }

        [DisplayName("Skill Description")]
        public string SkillDescription { get; set; }

        [Required(ErrorMessage ="Please enter the recommended hourly Rate for this skill")]
        [DisplayName("Recommended Rate")]
        public float Recom_Rate { get; set; }

        public virtual ICollection<WorkerSkill> WorkerSkills { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }




    }
}
