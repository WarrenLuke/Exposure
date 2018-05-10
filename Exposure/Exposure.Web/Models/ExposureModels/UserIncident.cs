using Exposure.Entities;
using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class UserIncident
    {
       
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        
        [Key, Column(Order = 2)]
        public int IncidentID { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("IncidentID")]
        public virtual Incident Incident { get; set; }
    }
}