namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateAdvert : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "DateAdvertised", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "DateAdvertised", c => c.DateTime());
        }
    }
}
