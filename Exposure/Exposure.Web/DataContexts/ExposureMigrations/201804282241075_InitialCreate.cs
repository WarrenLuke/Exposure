namespace Exposure.Web.DataContexts.ExposureMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        SubName = c.String(),
                        CityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SuburbID)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.GeneralBusinesses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        Slogan = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        WorkerID = c.Int(nullable: false),
                        Motivation = c.String(nullable: false, maxLength: 500),
                        Response = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        EmployerID = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(nullable: false, maxLength: 1024),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        HourlyRate = c.Double(nullable: false),
                        Rate = c.Single(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        CompletionDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobID);
            
            CreateTable(
                "dbo.JobHistories",
                c => new
                    {
                        JobHistoryID = c.Int(nullable: false, identity: true),
                        ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobHistoryID);
            
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        IncidentID = c.Int(nullable: false, identity: true),
                        Job = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1024),
                        Reporter = c.Int(nullable: false),
                        Offender = c.Int(nullable: false),
                        Progress = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IncidentID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        JobHistoryID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(nullable: false),
                        Reviewer = c.Int(nullable: false),
                        Reviewee = c.Int(nullable: false),
                        ReportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplications", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Suburbs", "CityID", "dbo.Cities");
            DropIndex("dbo.JobApplications", new[] { "JobID" });
            DropIndex("dbo.Suburbs", new[] { "CityID" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Incidents");
            DropTable("dbo.JobHistories");
            DropTable("dbo.Jobs");
            DropTable("dbo.JobApplications");
            DropTable("dbo.GeneralBusinesses");
            DropTable("dbo.Suburbs");
            DropTable("dbo.Cities");
        }
    }
}
