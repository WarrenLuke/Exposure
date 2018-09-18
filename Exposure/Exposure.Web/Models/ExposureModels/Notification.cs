using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class Notification
    {
        public int NotificationID { get; set; }

        public string Message { get; set; }

        public NoticeType notice { get; set; }

        [Column(TypeName="date")]
        public DateTime Updated { get; set; }

        [ForeignKey("Jobs")]
        public int? Job { get; set; }

        [ForeignKey("Incident")]
        public int? inc { get; set; }

        [ForeignKey("JobApplication")]
        public int? JobApp { get; set; }

        [ForeignKey("ApplicationUser")]
        public string User { get; set; }

        [DefaultValue(false)]
        public bool Flagged { get; set; }

        public virtual Incident Incident { get; set; }
        public virtual Job Jobs { get; set; }
        public virtual JobApplication JobApplication { get; set; }

        public virtual  ApplicationUser ApplicationUser { get; set; }
    }
}