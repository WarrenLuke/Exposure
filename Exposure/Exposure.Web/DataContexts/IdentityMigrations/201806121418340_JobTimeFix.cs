namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTimeFix : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Jobs DROP CONSTRAINT DF__Jobs__StartTime__6477ECF3");
            Sql("ALTER TABLE dbo.Jobs DROP CONSTRAINT DF__Jobs__EndTime__656C112C");
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
