namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Date : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
