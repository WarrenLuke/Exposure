namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobExpiration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ExpiryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "ExpiryDate");
        }
    }
}
