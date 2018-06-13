namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DA : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "DateAdvertised", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "DateAdvertised", c => c.DateTime(nullable: false));
        }
    }
}
