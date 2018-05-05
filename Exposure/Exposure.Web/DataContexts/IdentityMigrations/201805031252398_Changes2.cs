namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReviewJobApplications", "Review_ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.ReviewJobApplications", "JobApplication_JobApplicationID", "dbo.JobApplications");
            DropIndex("dbo.ReviewJobApplications", new[] { "Review_ReviewID" });
            DropIndex("dbo.ReviewJobApplications", new[] { "JobApplication_JobApplicationID" });
            AddColumn("dbo.Reviews", "JobApplication_JobApplicationID", c => c.Int());
            CreateIndex("dbo.Reviews", "JobApplication_JobApplicationID");
            AddForeignKey("dbo.Reviews", "JobApplication_JobApplicationID", "dbo.JobApplications", "JobApplicationID");
            DropTable("dbo.ReviewJobApplications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReviewJobApplications",
                c => new
                    {
                        Review_ReviewID = c.Int(nullable: false),
                        JobApplication_JobApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Review_ReviewID, t.JobApplication_JobApplicationID });
            
            DropForeignKey("dbo.Reviews", "JobApplication_JobApplicationID", "dbo.JobApplications");
            DropIndex("dbo.Reviews", new[] { "JobApplication_JobApplicationID" });
            DropColumn("dbo.Reviews", "JobApplication_JobApplicationID");
            CreateIndex("dbo.ReviewJobApplications", "JobApplication_JobApplicationID");
            CreateIndex("dbo.ReviewJobApplications", "Review_ReviewID");
            AddForeignKey("dbo.ReviewJobApplications", "JobApplication_JobApplicationID", "dbo.JobApplications", "JobApplicationID", cascadeDelete: true);
            AddForeignKey("dbo.ReviewJobApplications", "Review_ReviewID", "dbo.Reviews", "ReviewID", cascadeDelete: true);
        }
    }
}
