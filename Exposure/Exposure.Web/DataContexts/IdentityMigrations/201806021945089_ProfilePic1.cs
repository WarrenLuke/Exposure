namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfilePic1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "ProfilePic", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ProfilePic", c => c.Byte(nullable: false));
        }
    }
}
