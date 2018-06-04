namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JodTimeAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "StartTime", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "EndTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "EndTime");
            DropColumn("dbo.Jobs", "StartTime");
        }
    }
}
