using Exposure.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exposure.Web.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {





        public IdentityDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }

        public System.Data.Entity.DbSet<Exposure.Web.Models.AllUsers> AllUsers { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Job> Jobs { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Employer> Employers { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Skill> Skills { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Suburb> Suburbs { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Review> Reviews { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.JobApplication> JobApplications { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Incident> Incidents { get; set; }
    }
}
   
        
    