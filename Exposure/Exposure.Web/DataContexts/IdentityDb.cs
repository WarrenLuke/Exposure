using Exposure.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
            
        //    modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
        //    modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "dbo");
        //    modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");
        //    modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");
        //    modelBuilder.Entity<IdentityRole>().ToTable("Roles", "dbo");
        //}

        public System.Data.Entity.DbSet<Exposure.Entities.Job> Jobs { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Employer> Employers { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Skill> Skills { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Suburb> Suburbs { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Review> Reviews { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.JobApplication> JobApplications { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.Incident> Incidents { get; set; }

       

        public System.Data.Entity.DbSet<Exposure.Entities.Worker> Workers { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.City> Cities { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.WorkerSkill> WorkerSkills { get; set; }

        public System.Data.Entity.DbSet<Exposure.Entities.GeneralBusiness> GeneralBusinesses { get; set; }
    }
}
   
        
    