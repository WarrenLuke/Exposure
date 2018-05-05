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
        [Key]
        public int UserIncID { get; set; }
        
        public string UserId { get; set; }

        public int IncidentID { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Incident Incident { get; set; }


    }
}