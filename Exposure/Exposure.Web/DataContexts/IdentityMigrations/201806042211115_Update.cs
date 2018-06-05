namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobApplications", "Response", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobApplications", "Response", c => c.Int(nullable: false));
        }
    }
}
