using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class Job
    {
        [ScaffoldColumn(false)]
        public int JobID { get; set; }

        
        [DisplayName("Employer")]
        public int EmployerID { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide a description for your advertisement")]
        [StringLength(1024)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter a end date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please enter a start time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please enter a end time")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Please specify how much you will be paying]")]
        [Range(00.1, 100.00)]
        [DisplayName("Hourly Rate")]
        [DataType(DataType.Currency)]
        public double HourlyRate { get; set; }

        [DisplayName("Location")]
        [Required]
        public int SuburbID { get; set; }

        [DisplayName("Agreed Rate")]
        public float Rate { get; set; }

        [DefaultValue(false)]
        public bool Completed { get; set; }

        public virtual Suburb Suburb{ get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; }

       

        public DateTime? CompletionDate { get; set; }




    }
}
