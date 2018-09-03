namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobDateReq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "EndTime", c => c.DateTime());
            AlterColumn("dbo.Jobs", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime());
        }
    }
}
