
using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class Employer
    {
        [ForeignKey("ApplicationUser")]
        [Key]
        public string EmployerID { get; set; }

        public string WorkNo { get; set; }

        public int SuburbID { get; set; }

        public Suburb Suburb { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        
        //[ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}