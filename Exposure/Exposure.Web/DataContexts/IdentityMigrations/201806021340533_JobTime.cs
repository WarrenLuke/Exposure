namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Jobs", "StartTime");
            DropColumn("dbo.Jobs", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "EndTime", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "StartTime", c => c.Int(nullable: false));
        }
    }
}
