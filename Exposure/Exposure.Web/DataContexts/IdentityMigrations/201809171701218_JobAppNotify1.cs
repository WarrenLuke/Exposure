namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobAppNotify1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "JobApp", "dbo.JobApplications");
            DropIndex("dbo.Notifications", new[] { "JobApp" });
            AlterColumn("dbo.Notifications", "JobApp", c => c.Int());
            CreateIndex("dbo.Notifications", "JobApp");
            AddForeignKey("dbo.Notifications", "JobApp", "dbo.JobApplications", "JobApplicationID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "JobApp", "dbo.JobApplications");
            DropIndex("dbo.Notifications", new[] { "JobApp" });
            AlterColumn("dbo.Notifications", "JobApp", c => c.Int(nullable: false));
            CreateIndex("dbo.Notifications", "JobApp");
            AddForeignKey("dbo.Notifications", "JobApp", "dbo.JobApplications", "JobApplicationID", cascadeDelete: true);
        }
    }
}
