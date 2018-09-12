namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticeIncidentNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "incident", "dbo.Incidents");
            DropIndex("dbo.Notifications", new[] { "incident" });
            AlterColumn("dbo.Notifications", "incident", c => c.Int());
            CreateIndex("dbo.Notifications", "incident");
            AddForeignKey("dbo.Notifications", "incident", "dbo.Incidents", "IncidentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "incident", "dbo.Incidents");
            DropIndex("dbo.Notifications", new[] { "incident" });
            AlterColumn("dbo.Notifications", "incident", c => c.Int(nullable: false));
            CreateIndex("dbo.Notifications", "incident");
            AddForeignKey("dbo.Notifications", "incident", "dbo.Incidents", "IncidentID", cascadeDelete: true);
        }
    }
}
