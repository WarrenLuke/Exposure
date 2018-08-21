using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class Incident
    {
        [ScaffoldColumn(false)]
        public int IncidentID { get; set; }
          
        [ForeignKey("Job")]
        [Column(Order =1)]
        public int JobID { get; set; }

        [Required(ErrorMessage ="Please provide a descrtption of the incident")]
        [StringLength(1024)]
        public string Description { get; set; }

        [ForeignKey("Offender")]
        [Column(Order = 2)]
        public string OffenderID { get; set; }        
       
        public virtual ApplicationUser Offender { get; set; }

        public virtual ICollection<UserIncidents> UserIncidents { get; set; }

        public Job Job { get; set; }
        
        [DefaultValue("Pending")]
        public Progress Progress { get; set; }        

        public DateTime? ReportDate { get; set; }
    }
}
