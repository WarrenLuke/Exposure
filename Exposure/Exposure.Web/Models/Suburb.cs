using Exposure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exposure.Entities
{
    public class Suburb
    {
        public int SuburbID { get; set; }

        public string SubName { get; set; }

        public int CityID { get; set; }

        public virtual City City { get; set; }
      
        public virtual ICollection<ApplicationUser> applicationUsers { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }



    }
}
