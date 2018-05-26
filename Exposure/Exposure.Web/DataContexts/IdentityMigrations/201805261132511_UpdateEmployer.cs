namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmployer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employers", "WorkAddress1", c => c.String());
            AddColumn("dbo.Employers", "WorkAddress2", c => c.String());
            DropColumn("dbo.Employers", "WorkAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employers", "WorkAddress", c => c.String());
            DropColumn("dbo.Employers", "WorkAddress2");
            DropColumn("dbo.Employers", "WorkAddress1");
        }
    }
}
