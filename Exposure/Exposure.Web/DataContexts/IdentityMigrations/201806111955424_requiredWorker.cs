namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredWorker : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers");
            DropIndex("dbo.JobApplications", new[] { "WorkerID" });
            AlterColumn("dbo.JobApplications", "WorkerID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.JobApplications", "WorkerID");
            AddForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers", "WorkerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers");
            DropIndex("dbo.JobApplications", new[] { "WorkerID" });
            AlterColumn("dbo.JobApplications", "WorkerID", c => c.String(maxLength: 128));
            CreateIndex("dbo.JobApplications", "WorkerID");
            AddForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers", "WorkerID");
        }
    }
}
