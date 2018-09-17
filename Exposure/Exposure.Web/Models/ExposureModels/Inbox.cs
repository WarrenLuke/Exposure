using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class Inbox
    {
        public int InboxID { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Message { get; set; }

        public DateTime  Date { get; set; }

        public virtual  ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}