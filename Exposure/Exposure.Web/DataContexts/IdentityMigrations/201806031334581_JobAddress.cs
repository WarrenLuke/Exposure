namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "AddressLine1", c => c.String(nullable: false));
            AddColumn("dbo.Jobs", "AddressLine2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "AddressLine2");
            DropColumn("dbo.Jobs", "AddressLine1");
        }
    }
}
