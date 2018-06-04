namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
