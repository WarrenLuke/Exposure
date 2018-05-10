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
    public class Review
    {
        [ScaffoldColumn(false)]
        [Key]
        public int ReviewID { get; set; }
               
        [Required]      
        [Range(1,5, ErrorMessage ="Rating must be between {0} and {1}")]
        public double Rating { get; set; }

        [Required(ErrorMessage ="Please comment on the rating")]
        [StringLength(1024)]
        public string Comment { get; set; }

              
        public string UserId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public DateTime ReportDate { get; set; }

        public int JobApplicationID { get; set; }

        public virtual ICollection<UserReview> UserReviews { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        [ForeignKey("JobApplicationID")]
        public virtual JobApplication JobApplication { get; set; }



    }
}
