namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "Reviewee", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "Reviewee" });
            AlterColumn("dbo.Reviews", "Reviewee", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Reviews", "Reviewee");
            AddForeignKey("dbo.Reviews", "Reviewee", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Reviewee", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "Reviewee" });
            AlterColumn("dbo.Reviews", "Reviewee", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reviews", "Reviewee");
            AddForeignKey("dbo.Reviews", "Reviewee", "dbo.AspNetUsers", "Id");
        }
    }
}
