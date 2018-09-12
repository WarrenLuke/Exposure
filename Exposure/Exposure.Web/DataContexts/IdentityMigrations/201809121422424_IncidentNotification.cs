namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncidentNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "incident", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "Flagged", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Notifications", "incident");
            AddForeignKey("dbo.Notifications", "incident", "dbo.Incidents", "IncidentID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "incident", "dbo.Incidents");
            DropIndex("dbo.Notifications", new[] { "incident" });
            DropColumn("dbo.Notifications", "Flagged");
            DropColumn("dbo.Notifications", "incident");
        }
    }
}
