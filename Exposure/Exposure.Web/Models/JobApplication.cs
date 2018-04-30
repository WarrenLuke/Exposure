using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class JobApplication
    {
        [ScaffoldColumn(false)]
        public int  JobApplicationID { get; set; }

        [Required]
        public int JobID { get; set; }

        
        public int WorkerID { get; set; }

        [Required(ErrorMessage = "Provide a short motivation why you should be hired")]
        [StringLength(500)]
        public string Motivation { get; set; }

        public Reply Response  { get; set; }

        public virtual Job Job { get; set; }

        
    }
}
