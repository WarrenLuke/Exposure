using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class Review
    {
        [ScaffoldColumn(false)]
        public int ReviewID { get; set; }

        [DisplayName("Job")]
        [Required(ErrorMessage ="Please specify the worker you would like to review")]
        public int JobHistoryID { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required(ErrorMessage ="Please comment on the rating")]
        public string Comment { get; set; }

        public int Reviewer { get; set; }

        [Required(ErrorMessage ="Please select worker you would like to review")]
        public int Reviewee { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        
       
    }
}
