namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using Exposure.Entities;
    using Exposure.Web.Models;
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
            context.Cities.AddOrUpdate(
                cit => cit.CityName,
                new City() {CityName = "Port Elizabeth", CityAbbrev = "PE" });

            context.Suburbs.AddOrUpdate(
               sub => sub.SuburbID,
               new Suburb() { SubName = "Adcockvale", CityID = 1 },
               new Suburb() { SubName = "Algoa Bay", CityID = 1 },
               new Suburb() { SubName = "Algoa Park", CityID = 1 },
               new Suburb() { SubName = "Amsterdam Hoek", CityID = 1 },
               new Suburb() { SubName = "Arcadia", CityID = 1 },
               new Suburb() { SubName = "Benkamma", CityID = 1 },
               new Suburb() { SubName = "Bethelsdorp", CityID = 1 },
               new Suburb() { SubName = "Beverley Grove", CityID = 1 },
               new Suburb() { SubName = "Bloemendal", CityID = 1 },
               new Suburb() { SubName = "Bluewater Bay", CityID = 1 },
               new Suburb() { SubName = "Boastville", CityID = 1 },
               new Suburb() { SubName = "Bramhope", CityID = 1 },
               new Suburb() { SubName = "Broadwood", CityID = 1 },
               new Suburb() { SubName = "Cadles", CityID = 1 },
               new Suburb() { SubName = "Charlo", CityID = 1 },
               new Suburb() { SubName = "Chatty", CityID = 1 },
               new Suburb() { SubName = "Cleary Estate", CityID = 1 },
               new Suburb() { SubName = "Cotsworld", CityID = 1 },
               new Suburb() { SubName = "Dan Jooste Park", CityID = 1 },
               new Suburb() { SubName = "Dassie Kraal", CityID = 1 },
               new Suburb() { SubName = "Deal Party Estate", CityID = 1 },
               new Suburb() { SubName = "Dowerville", CityID = 1 },
               new Suburb() { SubName = "Driftsands", CityID = 1 },
               new Suburb() { SubName = "Duinen", CityID = 1 },
               new Suburb() { SubName = "Eastbourne Township", CityID = 1 },
               new Suburb() { SubName = "Elundini", CityID = 1 },
               new Suburb() { SubName = "Emerald Hill", CityID = 1 },
               new Suburb() { SubName = "Fairview", CityID = 1 },
               new Suburb() { SubName = "Fernglen", CityID = 1 },
               new Suburb() { SubName = "Ferguson Township", CityID = 1 },
               new Suburb() { SubName = "Fowler Township", CityID = 1 },
               new Suburb() { SubName = "Francis Evatt Park", CityID = 1 },
               new Suburb() { SubName = "Gelvandale", CityID = 1 },
               new Suburb() { SubName = "Gelvan Park", CityID = 1 },
               new Suburb() { SubName = "Gelvan Square", CityID = 1 },
               new Suburb() { SubName = "Gillets", CityID = 1 },
               new Suburb() { SubName = "Gipsonville", CityID = 1 },
               new Suburb() { SubName = "Glen Hurd", CityID = 1 },
               new Suburb() { SubName = "Glendinningvale", CityID = 1 },
               new Suburb() { SubName = "Goldwater", CityID = 1 },
               new Suburb() { SubName = "Greenshields Park", CityID = 1 },
               new Suburb() { SubName = "Hart Township", CityID = 1 },
               new Suburb() { SubName = "Hillskraal", CityID = 1 },
               new Suburb() { SubName = "Holland Park", CityID = 1 },
               new Suburb() { SubName = "Humewood", CityID = 1 },
               new Suburb() { SubName = "Idylwyde", CityID = 1 },
               new Suburb() { SubName = "Jarman Township", CityID = 1 },
               new Suburb() { SubName = "Jutland Township", CityID = 1 },
               new Suburb() { SubName = "Kabega Park", CityID = 1 },
               new Suburb() { SubName = "Kakkerlaksvlei", CityID = 1 },
               new Suburb() { SubName = "Kemsley Park", CityID = 1 },
               new Suburb() { SubName = "Kensington", CityID = 1 },
               new Suburb() { SubName = "Kleinskool", CityID = 1 },
               new Suburb() { SubName = "Korsten", CityID = 1 },
               new Suburb() { SubName = "Kragga Kamma", CityID = 1 },
               new Suburb() { SubName = "Kunene Park", CityID = 1 },
               new Suburb() { SubName = "Kwa Ford", CityID = 1 },
               new Suburb() { SubName = "Kwazakele", CityID = 1 },
               new Suburb() { SubName = "Lea Place", CityID = 1 },
               new Suburb() { SubName = "Linga Longa", CityID = 1 },
               new Suburb() { SubName = "Linkside", CityID = 1 },
               new Suburb() { SubName = "Linton Grange", CityID = 1 },
               new Suburb() { SubName = "Listerwood", CityID = 1 },
               new Suburb() { SubName = "Lorraine", CityID = 1 },
               new Suburb() { SubName = "Lovemore Heights", CityID = 1 },
               new Suburb() { SubName = "Malabar", CityID = 1 },
               new Suburb() { SubName = "Malatsky Valley", CityID = 1 },
               new Suburb() { SubName = "Mangold Park", CityID = 1 },
               new Suburb() { SubName = "Marais Township", CityID = 1 },
               new Suburb() { SubName = "Marchantdale", CityID = 1 },
               new Suburb() { SubName = "Markman Townshio", CityID = 1 },
               new Suburb() { SubName = "McLeanville", CityID = 1 },
               new Suburb() { SubName = "Mill Park", CityID = 1 },
               new Suburb() { SubName = "Millard Grange", CityID = 1 },
               new Suburb() { SubName = "Milner Township", CityID = 1 },
               new Suburb() { SubName = "Miramar", CityID = 1 },
               new Suburb() { SubName = "Missionvale", CityID = 1 },
               new Suburb() { SubName = "Moregrove", CityID = 1 },
               new Suburb() { SubName = "Mount Croix", CityID = 1 },
               new Suburb() { SubName = "Mount Pleasant", CityID = 1 },
               new Suburb() { SubName = "Neave Industrial Township", CityID = 1 },
               new Suburb() { SubName = "New Brighton", CityID = 1 },
               new Suburb() { SubName = "Newton Park", CityID = 1 },
               new Suburb() { SubName = "North End", CityID = 1 },
               new Suburb() { SubName = "Overbaaken", CityID = 1 },
               new Suburb() { SubName = "Pari Park", CityID = 1 },
               new Suburb() { SubName = "Park Side", CityID = 1 },
               new Suburb() { SubName = "Parsons Hill", CityID = 1 },
               new Suburb() { SubName = "Parsons Vlei", CityID = 1 },
               new Suburb() { SubName = "Perridgevale", CityID = 1 },
               new Suburb() { SubName = "Prince Nikiwe Township", CityID = 1 },
               new Suburb() { SubName = "Redhouse", CityID = 1 },
               new Suburb() { SubName = "Red Location", CityID = 1 },
               new Suburb() { SubName = "Retiefville", CityID = 1 },
               new Suburb() { SubName = "Rowallan Park", CityID = 1 },
               new Suburb() { SubName = "Rufane Vale", CityID = 1 },
               new Suburb() { SubName = "Rupert's Hope", CityID = 1 },
               new Suburb() { SubName = "Salisbury Park", CityID = 1 },
               new Suburb() { SubName = "Salsoneville", CityID = 1 },
               new Suburb() { SubName = "Salt Lake", CityID = 1 },
               new Suburb() { SubName = "Sanctor", CityID = 1 },
               new Suburb() { SubName = "Santa", CityID = 1 },
               new Suburb() { SubName = "Schauder Township", CityID = 1 },
               new Suburb() { SubName = "Schoenmaker's Kop", CityID = 1 },
               new Suburb() { SubName = "Scotstoun", CityID = 1 },
               new Suburb() { SubName = "Sidwell", CityID = 1 },
               new Suburb() { SubName = "South End", CityID = 1 },
               new Suburb() { SubName = "Southdene", CityID = 1 },
               new Suburb() { SubName = "Springfield", CityID = 1 },
               new Suburb() { SubName = "St. George's Strand", CityID = 1 },
               new Suburb() { SubName = "Steytler Township", CityID = 1 },
               new Suburb() { SubName = "Struanville", CityID = 1 },
               new Suburb() { SubName = "Stuart Township", CityID = 1 },
               new Suburb() { SubName = "Summerstrand", CityID = 1 },
               new Suburb() { SubName = "Sunridge Park", CityID = 1 },
               new Suburb() { SubName = "Sutton Vallance", CityID = 1 },
               new Suburb() { SubName = "Sydenham", CityID = 1 },
               new Suburb() { SubName = "Swartkops", CityID = 1 },
               new Suburb() { SubName = "Taybank", CityID = 1 },
               new Suburb() { SubName = "Theescombe", CityID = 1 },
               new Suburb() { SubName = "Thembaletu", CityID = 1 },
               new Suburb() { SubName = "Trevolen Township", CityID = 1 },
               new Suburb() { SubName = "Van Der Stel Township", CityID = 1 },
               new Suburb() { SubName = "Veeplaas", CityID = 1 },
               new Suburb() { SubName = "Viking Vale", CityID = 1 },
               new Suburb() { SubName = "Walmer", CityID = 1 },
               new Suburb() { SubName = "Wells Estate", CityID = 1 },
               new Suburb() { SubName = "West End", CityID = 1 },
               new Suburb() { SubName = "Westering", CityID = 1 },
               new Suburb() { SubName = "Westview", CityID = 1 },
               new Suburb() { SubName = "Weybridge Park", CityID = 1 },
               new Suburb() { SubName = "White Location", CityID = 1 },
               new Suburb() { SubName = "Willowdene", CityID = 1 },
               new Suburb() { SubName = "Winterstrand", CityID = 1 },
               new Suburb() { SubName = "Woodlands", CityID = 1 },
               new Suburb() { SubName = "Woolhope", CityID = 1 },
               new Suburb() { SubName = "Youngville", CityID = 1 },
               new Suburb() { SubName = "Zwide", CityID = 1 }

               );

            context.Skills.AddOrUpdate(
                sk => sk.SkillDescription,
                new Skill() { SkillDescription = "Painting", Recom_Rate = 10 },
                new Skill() { SkillDescription = "Gardening", Recom_Rate = 15 },
                new Skill() { SkillDescription = "Plumbing", Recom_Rate = 25 },
                new Skill() { SkillDescription = "Tiling", Recom_Rate = 30 },
                new Skill() { SkillDescription = "Welding", Recom_Rate = 35 },
                new Skill() { SkillDescription = "Carpentry", Recom_Rate = 40 }
                );

            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    FirstName = "Exposure",
                    LastName = "Admin",
                    AddressLine1 = "University Way",
                    Email = "admin@exposure.com",
                    UserName = "admin",
                    SuburbID = 1114,
                    Gender = "Male"
                };

                manager.Create(user, "ChangeItAsap!");
                manager.AddToRole(user.Id, "Admin");
            }








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
