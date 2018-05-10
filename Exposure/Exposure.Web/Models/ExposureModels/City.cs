using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class City
    {
        public int CityID { get; set; }

        [Required]
        public string CityName { get; set; }

        public string CityAbbrev { get; set; }

        public virtual ICollection<Suburb> Suburbs { get; set; }


    }
}
