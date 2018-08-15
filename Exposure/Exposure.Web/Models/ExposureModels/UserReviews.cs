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
        [Key]
        public int URID { get; set; }

        [Column(Order =1)]
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        [Column(Order =2)]
        [ForeignKey("Review")]
        public int ReviewID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Review Review { get; set; }
    }
}