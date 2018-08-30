namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralBusinesses", "CompanyBanner", c => c.Binary(storeType: "image"));
            AddColumn("dbo.GeneralBusinesses", "Logo", c => c.Binary(storeType: "image"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneralBusinesses", "Logo");
            DropColumn("dbo.GeneralBusinesses", "CompanyBanner");
        }
    }
}
