using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exposure.Web.Models.ViewModels
{
    public class EmailFormModel
    {
        [Required]
        public string Subject { get; set; }

        [Required, Display(Name = "Alias")]
        public string FromName { get; set; } 
        
        [Required, Display(Name = "From Email"), EmailAddress]
        public string FromEmail { get; set; }

        [Required, Display(Name ="Recipient"), EmailAddress]
        public string ToEmail { get; set; }

        [Required]
        public string Message { get; set; }
    }
}