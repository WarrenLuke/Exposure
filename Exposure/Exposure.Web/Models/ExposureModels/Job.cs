using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Exposure.Entities
{
    public class Job
    {
        [ScaffoldColumn(false)]
        public int JobID { get; set; }


        [DisplayName("Employer")]
        [ReadOnly(true)]
        public string EmployerID { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [DisplayName("Date Advertised")]
        public DateTime DateAdvertised { get; set; }

        [Required(ErrorMessage = "Each jobs needs to be associated with a skill")]
        [DisplayName("Category")]
        public int SkillID { get; set; }

        [Required(ErrorMessage = "Please provide a description for your advertisement")]
        [StringLength(1024)]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Required(ErrorMessage = "Please select a start date for the job")]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[CustomDateAttribute(ErrorMessage = "Please select a start date atleast 3 days from today.")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayName("End Date")]
        [Required(ErrorMessage = "Please select a end date for the job")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[EndDate(DateStartProperty = "StartDate", ErrorMessage = "End Date cannot be before start date")]
        public DateTime? EndDate { get; set; }


        [DisplayName("Start Time")]       
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Please select a start time for the job")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }


        [DisplayName("End Time")]        
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Please select an end time for your job")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Please specify how much you will be paying]")]
        [DisplayName("Rate")]
        [DataType(DataType.Currency)]
        public double Rate { get; set; }

        [Required]
        [DisplayName("Location")]
        public int SuburbID { get; set; }

        [Required]
        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }

        [DefaultValue(false)]
        public bool Completed { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public virtual Suburb Suburb { get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; }

        public virtual Skill Skill { get; set; }

        public virtual Employer Employer { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

    }

   
}
