namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        CityName = c.String(nullable: false),
                        CityAbbrev = c.String(),
                    })
                .PrimaryKey(t => t.CityID);
            
            CreateTable(
                "dbo.UserIncidents",
                c => new
                    {
                        UserIncID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        IncidentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserIncID)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Incidents", t => t.IncidentID, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.IncidentID);
            
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        IncidentID = c.Int(nullable: false, identity: true),
                        JobApplicationID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1024),
                        Reporter = c.String(nullable: false),
                        Offender = c.String(nullable: false),
                        Progress = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IncidentID)
                .ForeignKey("dbo.JobApplications", t => t.JobApplicationID, cascadeDelete: true)
                .Index(t => t.JobApplicationID);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        WorkerID = c.Int(nullable: false),
                        Motivation = c.String(nullable: false, maxLength: 500),
                        Status = c.Int(nullable: false),
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
                        Title = c.String(nullable: false, maxLength: 20),
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
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Double(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 1024),
                        ReportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID);
            
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        UserReviewID = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        ReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserReviewID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Reviews", t => t.ReviewID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ReviewID);
            
            CreateTable(
                "dbo.ReviewJobApplications",
                c => new
                    {
                        Review_ReviewID = c.Int(nullable: false),
                        JobApplication_JobApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Review_ReviewID, t.JobApplication_JobApplicationID })
                .ForeignKey("dbo.Reviews", t => t.Review_ReviewID, cascadeDelete: true)
                .ForeignKey("dbo.JobApplications", t => t.JobApplication_JobApplicationID, cascadeDelete: true)
                .Index(t => t.Review_ReviewID)
                .Index(t => t.JobApplication_JobApplicationID);
            
            AddColumn("dbo.AspNetUsers", "SuburbID", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Suburb_SuburbID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Suburb_SuburbID");
            AddForeignKey("dbo.AspNetUsers", "Suburb_SuburbID", "dbo.Suburbs", "SuburbID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserIncidents", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.UserReviews", "ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.UserReviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReviewJobApplications", "JobApplication_JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.ReviewJobApplications", "Review_ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.JobApplications", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Incidents", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.UserIncidents", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Suburbs", "CityID", "dbo.Cities");
            DropForeignKey("dbo.AspNetUsers", "Suburb_SuburbID", "dbo.Suburbs");
            DropIndex("dbo.ReviewJobApplications", new[] { "JobApplication_JobApplicationID" });
            DropIndex("dbo.ReviewJobApplications", new[] { "Review_ReviewID" });
            DropIndex("dbo.UserReviews", new[] { "ReviewID" });
            DropIndex("dbo.UserReviews", new[] { "UserId" });
            DropIndex("dbo.JobApplications", new[] { "JobID" });
            DropIndex("dbo.Incidents", new[] { "JobApplicationID" });
            DropIndex("dbo.UserIncidents", new[] { "IncidentID" });
            DropIndex("dbo.UserIncidents", new[] { "Id" });
            DropIndex("dbo.Suburbs", new[] { "CityID" });
            DropIndex("dbo.AspNetUsers", new[] { "Suburb_SuburbID" });
            DropColumn("dbo.AspNetUsers", "Suburb_SuburbID");
            DropColumn("dbo.AspNetUsers", "SuburbID");
            DropTable("dbo.ReviewJobApplications");
            DropTable("dbo.UserReviews");
            DropTable("dbo.Reviews");
            DropTable("dbo.Jobs");
            DropTable("dbo.JobApplications");
            DropTable("dbo.Incidents");
            DropTable("dbo.UserIncidents");
            DropTable("dbo.Cities");
            DropTable("dbo.Suburbs");
        }
    }
}
