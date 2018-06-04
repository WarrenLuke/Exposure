using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ReadOnly(true)]
        public string EmployerID { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        
        [DisplayName("Date Advertised")]
        public DateTime DateAdvertised { get; set; }

        [Required(ErrorMessage = "Each jobs needs to be associated with a skill")]
        [DisplayName("Category")]
        public int SkillID { get; set; }

        [Required(ErrorMessage = "Please provide a description for your advertisement")]
        [StringLength(1024)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a start date")]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please enter a end date")]
        [DataType(DataType.Date)]
        [DisplayName("Est. End Date")]
        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Please enter a start time")]
        
        [DisplayName("Start Time")]
        public int StartTime { get; set; }

        [Required(ErrorMessage = "Please enter a end time")]
        
        [DisplayName("End Time")]
        public int EndTime { get; set; }

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

        public virtual Suburb Suburb{ get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; }

        public virtual Skill Skill { get; set; }

        public virtual Employer Employer { get; set; }
        
       




    }
}
