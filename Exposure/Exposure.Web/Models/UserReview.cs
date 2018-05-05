using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class UserReview
    {
        [Key]
        public string UserReviewID { get; set; }

        
        public string UserId { get; set; }

        public int ReviewID { get; set; }

        public virtual Review Review { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser applicationUser { get; set; }


    }
}