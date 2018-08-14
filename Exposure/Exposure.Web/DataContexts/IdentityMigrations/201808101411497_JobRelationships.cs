namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications");
            DropIndex("dbo.Reviews", new[] { "JobApplicationID" });
            AddColumn("dbo.Incidents", "Job_JobID", c => c.Int());
            AddColumn("dbo.Reviews", "JobID", c => c.Int(nullable: false));
            CreateIndex("dbo.Incidents", "Job_JobID");
            CreateIndex("dbo.Reviews", "JobID");
            AddForeignKey("dbo.Incidents", "Job_JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.Reviews", "JobID", "dbo.Jobs", "JobID", cascadeDelete: true);
            DropColumn("dbo.Reviews", "JobApplicationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "JobApplicationID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Reviews", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Incidents", "Job_JobID", "dbo.Jobs");
            DropIndex("dbo.Reviews", new[] { "JobID" });
            DropIndex("dbo.Incidents", new[] { "Job_JobID" });
            DropColumn("dbo.Reviews", "JobID");
            DropColumn("dbo.Incidents", "Job_JobID");
            CreateIndex("dbo.Reviews", "JobApplicationID");
            AddForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications", "JobApplicationID", cascadeDelete: true);
        }
    }
}
