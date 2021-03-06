﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class JobApplication
    {
        
        public int  JobApplicationID { get; set; }

        [DisplayName("Job")]
        public int JobID { get; set; }

            
        [DisplayName("Worker")]
        [Required]
        public string WorkerID { get; set; }

        [Required(ErrorMessage = "Provide a short motivation why you should be hired")]
        [StringLength(500)]
        public string Motivation { get; set; }

        [DefaultValue("Pending")]
        public Reply? Response  { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ApplicationDate { get; set; }


        [DefaultValue(false)]
        public bool Flagged { get; set; }

        [DefaultValue(false)]
        public bool Replied { get; set; }

        public virtual ICollection<Notification> Notification { get; set; }

        public virtual Job Job { get; set; }

        public virtual Worker Worker { get; set; }

        



        


    }
}
