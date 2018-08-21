using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class UserIncidents
    {
        [Key]
        public int UIID { get; set; }

        [ForeignKey("Incident")]
        [Column(Order = 1)]
        public int IncidentID { get; set; }

        [ForeignKey("User")]
        [Column(Order =2)]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Incident Incident { get; set; }
    }
}