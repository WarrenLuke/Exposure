namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeNulls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Jobs", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
