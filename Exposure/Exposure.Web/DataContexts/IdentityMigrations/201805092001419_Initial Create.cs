namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employers",
                c => new
                {
                    EmployerID = c.String(nullable: false, maxLength: 128),
                    WorkNo = c.String(),
                    SuburbID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.EmployerID)
                .Index(t => t.EmployerID)
                .Index(t => t.SuburbID);

            AddForeignKey("dbo.Employers", "EmployerId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs", "SuburbID");


            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                    FirstName = c.String(maxLength: 100),
                    LastName = c.String(maxLength: 100),
                    AddressLine1 = c.String(maxLength: 1024),
                    AddressLine2 = c.String(),
                    SuburbID = c.Int(),
                    Discriminator = c.String(nullable: false, maxLength: 128),
                    
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.SuburbID);

            AddForeignKey("dbo.Users", "SuburbID", "dbo.Suburbs", "SuburbID", cascadeDelete: true);
                
                


            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserId);

            AddForeignKey("dbo.UserClaims", "UserId", "dbo.Users", "Id");

            CreateTable(
                "dbo.Incidents",
                c => new
                {
                    IncidentID = c.Int(nullable: false, identity: true),
                    JobApplicationID = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 1024),
                    Offender = c.String(nullable: false, maxLength: 128),
                    Progress = c.Int(nullable: false),

                })
                .PrimaryKey(t => t.IncidentID)
                .Index(t => t.JobApplicationID)
                .Index(t => t.Offender);

            AddForeignKey("dbo.Incidents", "Offender", "dbo.Users", "Id");
            AddForeignKey("dbo.Incidents", "JobApplicationID", "dbo.JobApplications", "JobApplicationID");
                

            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        WorkerID = c.String(nullable: false, maxLength: 128),
                        Motivation = c.String(nullable: false, maxLength: 500),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .Index(t => t.JobID)
                .Index(t => t.WorkerID);

            AddForeignKey("dbo.JobApplications", "JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers", "WorkerID");
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        EmployerID = c.String(nullable:false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 1024),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Rate = c.Double(nullable: false),
                        SuburbID = c.Int(nullable: false),
                        SkillID = c.Int(nullable: false),
                        AgreedRate = c.Single(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        CompletionDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobID)
                .Index(t => t.EmployerID)
                .Index(t => t.SuburbID)
                .Index(t => t.SkillID);

            AddForeignKey("dbo.Jobs", "EmployerID", "dbo.Employers", "EmployerID", cascadeDelete:true);
            AddForeignKey("dbo.Jobs", "SkillID", "dbo.Skills", "SkillID");
            AddForeignKey("dbo.Jobs", "SuburbID", "dbo.Suburbs", "SuburbID");




            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillID = c.Int(nullable: false, identity: true),
                        SkillDescription = c.String(),
                        Recom_Rate = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.SkillID);
            
            CreateTable(
                "dbo.WorkerSkills",
                c => new
                    {
                        WSID = c.Int(nullable: false, identity: true),
                        WorkerID = c.String(nullable: false, maxLength: 128),
                        SkillID = c.Int(nullable: false),
                        
                    })
                .PrimaryKey(t => t.WSID)
                .Index(t => t.SkillID)
                .Index(t => t.WorkerID);

            AddForeignKey("dbo.WorkerSkills", "SkillID", "dbo.Skills", "SkillID", cascadeDelete: true);
            AddForeignKey("dbo.WorkerSkills", "WorkerID", "dbo.Workers", "WorkerID", cascadeDelete:true);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        WorkerID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.WorkerID)
                .Index(t => t.WorkerID);

            AddForeignKey("dbo.Workers", "WorkerID", "dbo.Users", "Id", cascadeDelete: true);
            
            CreateTable(
                "dbo.Suburbs",
                c => new
                    {
                        SuburbID = c.Int(nullable: false, identity: true),
                        SubName = c.String(),
                        CityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SuburbID)
                .Index(t => t.CityID);

            AddForeignKey("dbo.Suburbs", "CityID", "dbo.Cities", "CityID");
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        CityName = c.String(nullable: false),
                        CityAbbrev = c.String(),
                    })
                .PrimaryKey(t => t.CityID);

            CreateTable(
                "dbo.Reviews",
                c => new
                {
                    ReviewID = c.Int(nullable: false, identity: true),
                    Rating = c.Double(nullable: false),
                    Comment = c.String(nullable: false, maxLength: 1024),
                    Reviewee = c.String(maxLength: 128),
                    ReportDate = c.DateTime(nullable: false),
                    JobApplicationID = c.Int(nullable: false),

                })
                .PrimaryKey(t => t.ReviewID)                    
                .Index(t => t.Reviewee)
                .Index(t => t.JobApplicationID);

            AddForeignKey("dbo.Reviews", "Reviewee", "dbo.Users", "Id");
            AddForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications", "JobApplicationID");

                           
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .Index(t => t.UserId);

            AddForeignKey("dbo.UserLogins", "UserId", "dbo.Users", "Id");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                
                .Index(t => t.RoleId)
                .Index(t => t.UserId);

            AddForeignKey("dbo.UserRoles", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.UserRoles", "RoleID", "dbo.Roles", "Id");


            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.Employers", "EmployerID", "dbo.Users");
            DropForeignKey("dbo.Reviews", "ApplicationUser_Id1", "dbo.Users");
            DropForeignKey("dbo.Reviews", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.Incidents", "ApplicationUser_Id1", "dbo.Users");
            DropForeignKey("dbo.Incidents", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers");
            DropForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.Users", "Review_ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.Reviews", "Reviewee", "dbo.Users");
            DropForeignKey("dbo.Jobs", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.Suburbs", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Users", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.WorkerSkills", "Worker_WorkerID", "dbo.Workers");
            DropForeignKey("dbo.Workers", "WorkerID", "dbo.Users");
            DropForeignKey("dbo.WorkerSkills", "SkillID", "dbo.Skills");
            DropForeignKey("dbo.Jobs", "SkillID", "dbo.Skills");
            DropForeignKey("dbo.Jobs", "EmployerID", "dbo.Employers");
            DropForeignKey("dbo.JobApplications", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Incidents", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.Users", "Incident_IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "Offender", "dbo.Users");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Reviews", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Reviews", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reviews", new[] { "JobApplicationID" });
            DropIndex("dbo.Reviews", new[] { "Reviewee" });
            DropIndex("dbo.Suburbs", new[] { "CityID" });
            DropIndex("dbo.Workers", new[] { "WorkerID" });
            DropIndex("dbo.WorkerSkills", new[] { "Worker_WorkerID" });
            DropIndex("dbo.WorkerSkills", new[] { "SkillID" });
            DropIndex("dbo.Jobs", new[] { "SkillID" });
            DropIndex("dbo.Jobs", new[] { "SuburbID" });
            DropIndex("dbo.Jobs", new[] { "EmployerID" });
            DropIndex("dbo.JobApplications", new[] { "WorkerID" });
            DropIndex("dbo.JobApplications", new[] { "JobID" });
            DropIndex("dbo.Incidents", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Incidents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Incidents", new[] { "Offender" });
            DropIndex("dbo.Incidents", new[] { "JobApplicationID" });
            DropIndex("dbo.UserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Users", new[] { "Review_ReviewID" });
            DropIndex("dbo.Users", new[] { "Incident_IncidentID" });
            DropIndex("dbo.Users", new[] { "SuburbID" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Employers", new[] { "SuburbID" });
            DropIndex("dbo.Employers", new[] { "EmployerID" });
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Reviews");
            DropTable("dbo.Cities");
            DropTable("dbo.Suburbs");
            DropTable("dbo.Workers");
            DropTable("dbo.WorkerSkills");
            DropTable("dbo.Skills");
            DropTable("dbo.Jobs");
            DropTable("dbo.JobApplications");
            DropTable("dbo.Incidents");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Employers");
        }
    }
}
