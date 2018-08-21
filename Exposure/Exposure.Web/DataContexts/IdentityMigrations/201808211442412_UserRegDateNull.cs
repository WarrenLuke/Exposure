namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRegDateNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "RegDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "RegDate", c => c.DateTime(nullable: false));
        }
    }
}
