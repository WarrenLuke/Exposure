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

        [Required]
        public string Offender { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        [ForeignKey("Offender")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public JobApplication JobApplications { get; set; }
        
        public Progress Progress { get; set; }
    }
}
