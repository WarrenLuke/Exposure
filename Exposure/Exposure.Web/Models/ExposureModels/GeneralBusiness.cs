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
    public class GeneralBusiness
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [DisplayName("Company Banner")]
        [Column(TypeName = "image")]        
        public byte[] CompanyBanner { get; set; }

        [DisplayName("Logo")]
        [Column(TypeName = "image")]
        public byte[] Logo { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required]
        public string Slogan { get; set; }

        [ForeignKey("Suburb")]
        public int SuburbID { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        [DisplayName("Fax No.")]
        [Required]
        public string FaxNo { get; set; }

        [DisplayName("Telephone No.")]
        [Required]
        public string TelNo { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }


        public virtual Suburb Suburb { get; set; }

    }
}
