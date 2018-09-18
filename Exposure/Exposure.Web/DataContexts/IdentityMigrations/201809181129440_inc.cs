namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inc : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "incident", newName: "inc");
            RenameIndex(table: "dbo.Notifications", name: "IX_incident", newName: "IX_inc");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Notifications", name: "IX_inc", newName: "IX_incident");
            RenameColumn(table: "dbo.Notifications", name: "inc", newName: "incident");
        }
    }
}
