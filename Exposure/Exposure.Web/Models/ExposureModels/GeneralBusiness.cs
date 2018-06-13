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

        [DisplayName("Company Logo")]
        [Column(TypeName = "image")]
        public byte[] CompanyLogo { get; set; }

        [DisplayName("Logo")]
        [Column(TypeName = "image")]
        public byte[] Logo { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required]
        public string Slogan { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        //public  Logo { get; set; }

    }
}
