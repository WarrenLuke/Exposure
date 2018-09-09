using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Exposure.Entities
{
    public class Help
    {
        public int HelpID { get; set; }
       
        [DisplayName("Question")]
        public string HelpQuestion { get; set; }

        [DisplayName("Answer")]
        public string HelpAnswer { get; set; }
    }
}