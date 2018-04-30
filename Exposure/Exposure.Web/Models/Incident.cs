﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class Incident
    {
        [ScaffoldColumn(false)]
        public int IncidentID { get; set; }

        [Required(ErrorMessage ="Please select a job from your history")]
        public int Job { get; set; }

        [Required(ErrorMessage ="Please provide a descrtption of the incident")]
        [StringLength(1024)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public int Reporter { get; set; }

        [Required]
        [DisplayName("Worker")]
        public int Offender { get; set; }


        public Progress Progress { get; set; }
    }
}
