
using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [ScaffoldColumn(false)]
        public string EmployerID { get; set; }

        [DisplayName("Company Name")]
        public string WorkName { get; set; }

        [DisplayName("Company Number")]
        public string WorkNumber { get; set; }

        [DisplayName("Company Address Line 1")]
        public string WorkAddress1 { get; set; }

        [DisplayName("Company Address Line 2")]
        public string WorkAddress2 { get; set; }

        [DisplayName("Company Location")]
        public int? SuburbID { get; set; }

        public Suburb Suburb { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        
        //[ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}