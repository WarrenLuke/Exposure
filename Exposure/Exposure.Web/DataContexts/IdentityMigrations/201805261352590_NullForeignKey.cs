namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs");
            DropIndex("dbo.Employers", new[] { "SuburbID" });
            AlterColumn("dbo.Employers", "SuburbID", c => c.Int());
            CreateIndex("dbo.Employers", "SuburbID");
            AddForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs", "SuburbID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs");
            DropIndex("dbo.Employers", new[] { "SuburbID" });
            AlterColumn("dbo.Employers", "SuburbID", c => c.Int(nullable: false));
            CreateIndex("dbo.Employers", "SuburbID");
            AddForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs", "SuburbID", cascadeDelete: true);
        }
    }
}
