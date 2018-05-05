using Exposure.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exposure.Web.DataContexts
{
    public class ExposureDb:DbContext
    {
        public ExposureDb()
            :base("DefaultConnection")
        {
            
        }
        public DbSet<City> Cities { get; set; }

        public DbSet<Suburb> Suburbs { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Incident> MyProperty { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<GeneralBusiness> General { get; set; }

    
    }
}