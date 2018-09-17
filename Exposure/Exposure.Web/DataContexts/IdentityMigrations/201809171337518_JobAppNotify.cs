namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobAppNotify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "JobApp", c => c.Int(nullable: false));
            CreateIndex("dbo.Notifications", "JobApp");
            AddForeignKey("dbo.Notifications", "JobApp", "dbo.JobApplications", "JobApplicationID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "JobApp", "dbo.JobApplications");
            DropIndex("dbo.Notifications", new[] { "JobApp" });
            DropColumn("dbo.Notifications", "JobApp");
        }
    }
}
