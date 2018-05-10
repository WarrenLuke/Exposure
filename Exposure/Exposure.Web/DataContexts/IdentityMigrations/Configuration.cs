namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using Exposure.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Exposure.Web.DataContexts.IdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\IdentityMigrations";
        }

        protected override void Seed(Exposure.Web.DataContexts.IdentityDb context)
        {

            if(!context.Roles.Any(r=>r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Employer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Employer" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Worker"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Worker" };

                manager.Create(role);
            }

            context.Suburbs.AddOrUpdate(
                s => s.SubName,
                new Suburb()
                {
                    SubName = "Walmer",
                    CityID = 1
                }
                );

            context.Skills.AddOrUpdate(
                sk => sk.SkillDescription,
                new Skill() { SkillDescription = "Painting", Recom_Rate = 250 },
                new Skill() { SkillDescription = "Gardening", Recom_Rate = 150 },
                new Skill() { SkillDescription = "Plumbing", Recom_Rate = 250 },
                new Skill() { SkillDescription = "Tiling", Recom_Rate = 300 },
                new Skill() { SkillDescription = "Welding", Recom_Rate = 350 },
                new Skill() { SkillDescription = "Tinkering", Recom_Rate = 100 }              

                );

            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
