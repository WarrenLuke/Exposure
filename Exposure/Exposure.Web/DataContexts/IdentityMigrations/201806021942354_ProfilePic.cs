namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfilePic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePic", c => c.Byte(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProfilePic");
        }
    }
}
