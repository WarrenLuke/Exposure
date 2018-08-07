namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobAppReplied : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "Replied", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "Replied");
        }
    }
}
