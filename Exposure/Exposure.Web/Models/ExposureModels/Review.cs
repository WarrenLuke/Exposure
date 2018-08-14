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
        public Review()
        {
            ReportDate = DateTime.UtcNow;
        }

        [ScaffoldColumn(false)]
        [Key, Column(Order = 0)]
        public int ReviewID { get; set; }
               
        [Required]      
        [Range(1,5, ErrorMessage ="Rating must be between {0} and {1}")]
        public double Rating { get; set; }

        [Required(ErrorMessage ="Please comment on the rating")]
        [StringLength(1024)]
        public string Comment { get; set; }        

        [ForeignKey("ApplicationUser")]
        [Required]
        public string Reviewee { get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime ReportDate { get; set; }

        public int JobID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<UserReviews> UserReviews { get; set; }

        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }



    }

    
}
