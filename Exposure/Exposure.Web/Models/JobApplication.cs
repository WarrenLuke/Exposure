using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class JobApplication
    {
        

        [Required]
        public int  JobApplicationID { get; set; }

        [DisplayName("Job")]
        public int JobID { get; set; }

        [Required]     
        [DisplayName("Worker")]
        public int WorkerID { get; set; }

        [Required(ErrorMessage = "Provide a short motivation why you should be hired")]
        [StringLength(500)]
        public string Motivation { get; set; }

        public Reply Status  { get; set; }

        public virtual Job Job { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; }

        public virtual ICollection<Review>  Reviews { get; set; }

        


    }
}
