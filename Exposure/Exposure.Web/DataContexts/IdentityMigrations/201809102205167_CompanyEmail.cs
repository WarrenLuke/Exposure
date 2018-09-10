namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralBusinesses", "EmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.GeneralBusinesses", "FaxNo", c => c.String(nullable: false));
            AlterColumn("dbo.GeneralBusinesses", "TelNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GeneralBusinesses", "TelNo", c => c.String());
            AlterColumn("dbo.GeneralBusinesses", "FaxNo", c => c.String());
            DropColumn("dbo.GeneralBusinesses", "EmailAddress");
        }
    }
}
