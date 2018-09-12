namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobNotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Job", c => c.Int());
            CreateIndex("dbo.Notifications", "Job");
            AddForeignKey("dbo.Notifications", "Job", "dbo.Jobs", "JobID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Job", "dbo.Jobs");
            DropIndex("dbo.Notifications", new[] { "Job" });
            DropColumn("dbo.Notifications", "Job");
        }
    }
}
