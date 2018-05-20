namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Unknown : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employers", "WorkName", c => c.String());
            AddColumn("dbo.Employers", "WorkNumber", c => c.String());
            AddColumn("dbo.Employers", "WorkAddress", c => c.String());
            DropColumn("dbo.Employers", "WorkNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employers", "WorkNo", c => c.String());
            DropColumn("dbo.Employers", "WorkAddress");
            DropColumn("dbo.Employers", "WorkNumber");
            DropColumn("dbo.Employers", "WorkName");
        }
    }
}
