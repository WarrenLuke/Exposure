using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class JobHistory
    {
        [ScaffoldColumn(false)]
        public int JobHistoryID { get; set; }

        [DisplayName("Job")]
        [Required]
        public int ApplicationID { get; set; }

        
        

        
        

    }
}
