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

        [Required(ErrorMessage ="Please select a job from your history")]
        public int JobApplicationID { get; set; }

        [Required(ErrorMessage ="Please provide a descrtption of the incident")]
        [StringLength(1024)]
        public string Description { get; set; }

        [ForeignKey("Offender")]
        [Column(Order = 1)]
        public string OffenderID { get; set; }

        [ForeignKey("Reporter")]
        [Column(Order = 2)]
        public string ReporterID { get; set; }
       
        public virtual ApplicationUser Offender { get; set; }
        public virtual ApplicationUser Reporter { get; set; }

        public JobApplication JobApplications { get; set; }
        
        public Progress Progress { get; set; }
    }
}
