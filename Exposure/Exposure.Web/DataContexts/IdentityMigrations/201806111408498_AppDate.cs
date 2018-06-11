namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "ApplicationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "ApplicationDate");
        }
    }
}
