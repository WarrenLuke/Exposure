namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppDateNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobApplications", "ApplicationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobApplications", "ApplicationDate", c => c.DateTime(nullable: false));
        }
    }
}
