using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class UserReviews
    {
        [Key, Column(Order =0)]
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        [Key, Column(Order =1)]
        [ForeignKey("Review")]
        public int ReviewID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Review Review { get; set; }
    }
}