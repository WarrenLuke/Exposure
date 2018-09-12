namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "User", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "User");
            AddForeignKey("dbo.Notifications", "User", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "User", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "User" });
            DropColumn("dbo.Notifications", "User");
        }
    }
}
