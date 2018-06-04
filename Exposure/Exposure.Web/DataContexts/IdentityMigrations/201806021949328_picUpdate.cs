namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ProfilePic");
            AddColumn("dbo.AspNetUsers", "ProfilePic", c => c.Binary(storeType: "image"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProfilePic");
        }
    }
}
