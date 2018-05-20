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
                sub => sub.SubName,
                new Suburb() { SubName = "ADCOCKVALE", CityID = 1 },
                new Suburb() { SubName = "ALGOA BAY", CityID = 1 },
                new Suburb() { SubName = "ALGOA PARK", CityID = 1 },
                new Suburb() { SubName = "AMSTERDAM HOEK", CityID = 1 },
                new Suburb() { SubName = "ARCADIA", CityID = 1 },
                new Suburb() { SubName = "BENKAMMA", CityID = 1 },
                new Suburb() { SubName = "BETHELSDORP", CityID = 1 },
                new Suburb() { SubName = "BEVERLEY GROVE", CityID = 1 },
                new Suburb() { SubName = "BLOEMENDAL", CityID = 1 },
                new Suburb() { SubName = "BLUEWATER BAY", CityID = 1 },
                new Suburb() { SubName = "BOASTVILLE", CityID = 1 },
                new Suburb() { SubName = "BRAMHOPE", CityID = 1 },
                new Suburb() { SubName = "BROADWOOD", CityID = 1 },
                new Suburb() { SubName = "CADLES", CityID = 1 },
                new Suburb() { SubName = "CHARLO", CityID = 1 },
                new Suburb() { SubName = "CHATTY", CityID = 1 },
                new Suburb() { SubName = "CLEARY ESTATE", CityID = 1 },
                new Suburb() { SubName = "COTSWOLD", CityID = 1 },
                new Suburb() { SubName = "DAN JOOSTE PARK", CityID = 1 },
                new Suburb() { SubName = "DASSIE KRAAL", CityID = 1 },
                new Suburb() { SubName = "DEAL PARTY ESTATE", CityID = 1 },
                new Suburb() { SubName = "DOWERVILLE", CityID = 1 },
                new Suburb() { SubName = "DRIFTSANDS", CityID = 1 },
                new Suburb() { SubName = "DUINEN", CityID = 1 },
                new Suburb() { SubName = "EASTBOURNE TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "ELUNDINI", CityID = 1 },
                new Suburb() { SubName = "EMERALD HILL", CityID = 1 },
                new Suburb() { SubName = "FAIRVIEW", CityID = 1 },
                new Suburb() { SubName = "FERNGLEN", CityID = 1 },
                new Suburb() { SubName = "FERGUSON TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "FOWLER TOWNSHIPL", CityID = 1 },
                new Suburb() { SubName = "FRANCIS EVATT PARK", CityID = 1 },
                new Suburb() { SubName = "GELVANDALE", CityID = 1 },
                new Suburb() { SubName = "GELVAN PARK", CityID = 1 },
                new Suburb() { SubName = "GELVAN SQUARE", CityID = 1 },
                new Suburb() { SubName = "GILLETS", CityID = 1 },
                new Suburb() { SubName = "GIPSONVILLE", CityID = 1 },
                new Suburb() { SubName = "GLEN HURD", CityID = 1 },
                new Suburb() { SubName = "GLENDINNINGVALE", CityID = 1 },
                new Suburb() { SubName = "GOLDWATER", CityID = 1 },
                new Suburb() { SubName = "GREENSHIELDS PARK", CityID = 1 },
                new Suburb() { SubName = "HART TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "HILLSKRAAL", CityID = 1 },
                new Suburb() { SubName = "HOLLAND PARK", CityID = 1 },
                new Suburb() { SubName = "HUMEWOOD", CityID = 1 },
                new Suburb() { SubName = "IDYLWYLDE", CityID = 1 },
                new Suburb() { SubName = "JARMAN TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "JUTLAND TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "KABEGA PARK", CityID = 1 },
                new Suburb() { SubName = "KAKKERLAKSVLEI", CityID = 1 },
                new Suburb() { SubName = "KEMSLEY PARK", CityID = 1 },
                new Suburb() { SubName = "KENSINGTON", CityID = 1 },
                new Suburb() { SubName = "KLEINSKOOL", CityID = 1 },
                new Suburb() { SubName = "KORSTEN", CityID = 1 },
                new Suburb() { SubName = "KRAGGA KAMMA", CityID = 1 },
                new Suburb() { SubName = "KUNENE PARK", CityID = 1 },
                new Suburb() { SubName = "KWA FORD", CityID = 1 },
                new Suburb() { SubName = "KWAZAKELE", CityID = 1 },
                new Suburb() { SubName = "LEA PLACE", CityID = 1 },
                new Suburb() { SubName = "LINGA LONGA", CityID = 1 },
                new Suburb() { SubName = "LINKSIDE", CityID = 1 },
                new Suburb() { SubName = "LINTON GRANGE", CityID = 1 },
                new Suburb() { SubName = "LISTERWOOD", CityID = 1 },
                new Suburb() { SubName = "LORRAINE", CityID = 1 },
                new Suburb() { SubName = "LOVEMORE HEIGHTS", CityID = 1 },
                new Suburb() { SubName = "MALABAR", CityID = 1 },
                new Suburb() { SubName = "MALATSKY VALLEY", CityID = 1 },
                new Suburb() { SubName = "MANGOLD PARK", CityID = 1 },
                new Suburb() { SubName = "MARAIS TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "MARCHANTDALE", CityID = 1 },
                new Suburb() { SubName = "MARKMAN TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "McLEANVILLE", CityID = 1 },
                new Suburb() { SubName = "MILL PARK", CityID = 1 },
                new Suburb() { SubName = "MILLARD GRANGE", CityID = 1 },
                new Suburb() { SubName = "MILNER TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "MIRAMAR", CityID = 1 },
                new Suburb() { SubName = "MISSIONVALE", CityID = 1 },
                new Suburb() { SubName = "MOREGROVE", CityID = 1 },
                new Suburb() { SubName = "MOUNT CROIX", CityID = 1 },
                new Suburb() { SubName = "MOUNT PLEASANT", CityID = 1 },
                new Suburb() { SubName = "NEAVE INDUSTRIAL TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "NEW BRIGHTON", CityID = 1 },
                new Suburb() { SubName = "NEWTON PARK", CityID = 1 },
                new Suburb() { SubName = "NORTH END", CityID = 1 },
                new Suburb() { SubName = "OVERBAAKEN", CityID = 1 },
                new Suburb() { SubName = "PARI PARK", CityID = 1 },
                new Suburb() { SubName = "PARK SIDE", CityID = 1 },
                new Suburb() { SubName = "PARSONS HILL", CityID = 1 },
                new Suburb() { SubName = "PARSONS VLEI", CityID = 1 },
                new Suburb() { SubName = "PERRIDGEVALE", CityID = 1 },
                new Suburb() { SubName = "PRINCE NIKIWE TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "REDHOUSE", CityID = 1 },
                new Suburb() { SubName = "RED LOCATION", CityID = 1 },
                new Suburb() { SubName = "RETIEFVILLE", CityID = 1 },
                new Suburb() { SubName = "ROWALLAN PARK", CityID = 1 },
                new Suburb() { SubName = "RUFANE VALE", CityID = 1 },
                new Suburb() { SubName = "RUPERT'S HOPE", CityID = 1 },
                new Suburb() { SubName = "SALISBURY PARK", CityID = 1 },
                new Suburb() { SubName = "SALSONEVILLE", CityID = 1 },
                new Suburb() { SubName = "SALT LAKE", CityID = 1 },
                new Suburb() { SubName = "SANCTOR", CityID = 1 },
                new Suburb() { SubName = "SANTA", CityID = 1 },
                new Suburb() { SubName = "SCHAUDER TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "SCHOENMAKER'S KOP", CityID = 1 },
                new Suburb() { SubName = "SCOTSTOUN", CityID = 1 },
                new Suburb() { SubName = "SIDWELL", CityID = 1 },
                new Suburb() { SubName = "SOUTH END", CityID = 1 },
                new Suburb() { SubName = "SOUTHDENE", CityID = 1 },
                new Suburb() { SubName = "SPRINGFIELD", CityID = 1 },
                new Suburb() { SubName = "ST. GEORGE'S STRAND", CityID = 1 },
                new Suburb() { SubName = "STEYTLER TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "STRUANVILLE", CityID = 1 },
                new Suburb() { SubName = "STUART TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "SUMMERSTRAND", CityID = 1 },
                new Suburb() { SubName = "SUNRIDGE PARK", CityID = 1 },
                new Suburb() { SubName = "SUTTON VALLANCE", CityID = 1 },
                new Suburb() { SubName = "SYDENHAM", CityID = 1 },
                new Suburb() { SubName = "SWARTKOPS", CityID = 1 },
                new Suburb() { SubName = "TAYBANK", CityID = 1 },
                new Suburb() { SubName = "THEESCOMBE", CityID = 1 },
                new Suburb() { SubName = "THEMBALETU", CityID = 1 },
                new Suburb() { SubName = "TREVOLEN TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "VAN DER STEL TOWNSHIP", CityID = 1 },
                new Suburb() { SubName = "VEEPLAAS", CityID = 1 },
                new Suburb() { SubName = "VIKING VALE", CityID = 1 },
                new Suburb() { SubName = "WALMER", CityID = 1 },
                new Suburb() { SubName = "WELLS ESTATE", CityID = 1 },
                new Suburb() { SubName = "WEST END", CityID = 1 },
                new Suburb() { SubName = "WESTERING", CityID = 1 },
                new Suburb() { SubName = "WESTVIEW", CityID = 1 },
                new Suburb() { SubName = "WEYBRIDGE PARK", CityID = 1 },
                new Suburb() { SubName = "WHITE LOCATION", CityID = 1 },
                new Suburb() { SubName = "WILLOWDENE", CityID = 1 },
                new Suburb() { SubName = "WINTERSTRAND", CityID = 1 },
                new Suburb() { SubName = "WOODLANDS", CityID = 1 },
                new Suburb() { SubName = "WOOLHOPE", CityID = 1 },
                new Suburb() { SubName = "YOUNGVILLE", CityID = 1 },
                new Suburb() { SubName = "ZWIDE", CityID = 1 }

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
