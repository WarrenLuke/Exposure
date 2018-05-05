using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class Worker
    {
        [ForeignKey("ApplicationUser")]
        [Key]
        public string WorkerID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<WorkerSkill> WorkerSkills { get; set; }




    }
}