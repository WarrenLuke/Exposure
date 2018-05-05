namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "SuburbID", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "SuburbID");
            AddForeignKey("dbo.Jobs", "SuburbID", "dbo.Suburbs", "SuburbID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "SuburbID", "dbo.Suburbs");
            DropIndex("dbo.Jobs", new[] { "SuburbID" });
            DropColumn("dbo.Jobs", "SuburbID");
        }
    }
}
