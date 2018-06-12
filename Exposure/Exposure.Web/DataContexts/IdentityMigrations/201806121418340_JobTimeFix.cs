namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTimeFix : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Jobs DROP CONSTRAINT DF__Jobs__StartTime__4D5F7D71");
            Sql("ALTER TABLE dbo.Jobs DROP CONSTRAINT DF__Jobs__EndTime__4E53A1AA");
            AlterColumn("dbo.Jobs", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobs", "StartTime", c => c.Int(nullable: false));
        }
    }
}
