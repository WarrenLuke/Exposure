namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Suburbs",
                c => new
                    {
                        SuburbID = c.Int(nullable: false, identity: true),
                        SubName = c.String(nullable: false),
                        CityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SuburbID)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Gender = c.String(nullable: false, maxLength: 10),
                        AddressLine1 = c.String(nullable: false, maxLength: 1024),
                        AddressLine2 = c.String(),
                        SuburbID = c.Int(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suburbs", t => t.SuburbID, cascadeDelete: true)
                .Index(t => t.SuburbID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Employers",
                c => new
                    {
                        EmployerID = c.String(nullable: false, maxLength: 128),
                        WorkNo = c.String(),
                        SuburbID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployerID)
                .ForeignKey("dbo.AspNetUsers", t => t.EmployerID)
                .ForeignKey("dbo.Suburbs", t => t.SuburbID, cascadeDelete: true)
                .Index(t => t.EmployerID)
                .Index(t => t.SuburbID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        EmployerID = c.String(maxLength: 128),
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
                .ForeignKey("dbo.Skills", t => t.SkillID, cascadeDelete: true)
                .ForeignKey("dbo.Employers", t => t.EmployerID)
                .ForeignKey("dbo.Suburbs", t => t.SuburbID, cascadeDelete: true)
                .Index(t => t.EmployerID)
                .Index(t => t.SuburbID)
                .Index(t => t.SkillID);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        WorkerID = c.String(maxLength: 128),
                        Motivation = c.String(nullable: false, maxLength: 500),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .ForeignKey("dbo.Workers", t => t.WorkerID)
                .Index(t => t.JobID)
                .Index(t => t.WorkerID);
            
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        IncidentID = c.Int(nullable: false, identity: true),
                        JobApplicationID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1024),
                        UserId = c.String(maxLength: 128),
                        Progress = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IncidentID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.JobApplications", t => t.JobApplicationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.JobApplicationID)
                .Index(t => t.UserId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.UserIncidents",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        IncidentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.IncidentID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Incidents", t => t.IncidentID, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.IncidentID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Double(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 1024),
                        UserId = c.String(maxLength: 128),
                        ReportDate = c.DateTime(nullable: false),
                        JobApplicationID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.JobApplications", t => t.JobApplicationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.UserId)
                .Index(t => t.JobApplicationID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReviewID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Reviews", t => t.ReviewID, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ReviewID);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        WorkerID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.WorkerID)
                .ForeignKey("dbo.AspNetUsers", t => t.WorkerID)
                .Index(t => t.WorkerID);
            
            CreateTable(
                "dbo.WorkerSkills",
                c => new
                    {
                        WSID = c.Int(nullable: false, identity: true),
                        WorkerID = c.Int(nullable: false),
                        SkillID = c.Int(nullable: false),
                        Worker_WorkerID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WSID)
                .ForeignKey("dbo.Skills", t => t.SkillID, cascadeDelete: true)
                .ForeignKey("dbo.Workers", t => t.Worker_WorkerID)
                .Index(t => t.SkillID)
                .Index(t => t.Worker_WorkerID);
            
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
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
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
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Suburbs", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Reviews", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.Jobs", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.Jobs", "EmployerID", "dbo.Employers");
            DropForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers");
            DropForeignKey("dbo.WorkerSkills", "Worker_WorkerID", "dbo.Workers");
            DropForeignKey("dbo.WorkerSkills", "SkillID", "dbo.Skills");
            DropForeignKey("dbo.Jobs", "SkillID", "dbo.Skills");
            DropForeignKey("dbo.Workers", "WorkerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserReviews", "ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.UserReviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobApplications", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.UserIncidents", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.UserIncidents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.Incidents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employers", "EmployerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.WorkerSkills", new[] { "Worker_WorkerID" });
            DropIndex("dbo.WorkerSkills", new[] { "SkillID" });
            DropIndex("dbo.Workers", new[] { "WorkerID" });
            DropIndex("dbo.UserReviews", new[] { "ReviewID" });
            DropIndex("dbo.UserReviews", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Reviews", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reviews", new[] { "JobApplicationID" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.UserIncidents", new[] { "IncidentID" });
            DropIndex("dbo.UserIncidents", new[] { "UserId" });
            DropIndex("dbo.Incidents", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Incidents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Incidents", new[] { "UserId" });
            DropIndex("dbo.Incidents", new[] { "JobApplicationID" });
            DropIndex("dbo.JobApplications", new[] { "WorkerID" });
            DropIndex("dbo.JobApplications", new[] { "JobID" });
            DropIndex("dbo.Jobs", new[] { "SkillID" });
            DropIndex("dbo.Jobs", new[] { "SuburbID" });
            DropIndex("dbo.Jobs", new[] { "EmployerID" });
            DropIndex("dbo.Employers", new[] { "SuburbID" });
            DropIndex("dbo.Employers", new[] { "EmployerID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "SuburbID" });
            DropIndex("dbo.Suburbs", new[] { "CityID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Skills");
            DropTable("dbo.WorkerSkills");
            DropTable("dbo.Workers");
            DropTable("dbo.UserReviews");
            DropTable("dbo.Reviews");
            DropTable("dbo.UserIncidents");
            DropTable("dbo.Incidents");
            DropTable("dbo.JobApplications");
            DropTable("dbo.Jobs");
            DropTable("dbo.Employers");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Suburbs");
            DropTable("dbo.Cities");
        }
    }
}
