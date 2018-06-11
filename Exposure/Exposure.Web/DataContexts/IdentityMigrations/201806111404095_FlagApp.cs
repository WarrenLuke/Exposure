namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlagApp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "Flagged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "Flagged");
        }
    }
}
