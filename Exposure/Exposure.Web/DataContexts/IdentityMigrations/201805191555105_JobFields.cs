namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "Response", c => c.Int(nullable: false));
            DropColumn("dbo.Jobs", "AgreedRate");
            DropColumn("dbo.JobApplications", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobApplications", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "AgreedRate", c => c.Single(nullable: false));
            DropColumn("dbo.JobApplications", "Response");
        }
    }
}
