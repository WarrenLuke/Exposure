namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneralBus1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralBusinesses", "SuburbID", c => c.Int(nullable: false));
            AddColumn("dbo.GeneralBusinesses", "FaxNo", c => c.String());
            AddColumn("dbo.GeneralBusinesses", "TelNo", c => c.String());
            CreateIndex("dbo.GeneralBusinesses", "SuburbID");
            AddForeignKey("dbo.GeneralBusinesses", "SuburbID", "dbo.Suburbs", "SuburbID", cascadeDelete: true);
            DropColumn("dbo.GeneralBusinesses", "CompanyLogo");
            DropColumn("dbo.GeneralBusinesses", "Logo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneralBusinesses", "Logo", c => c.Binary(storeType: "image"));
            AddColumn("dbo.GeneralBusinesses", "CompanyLogo", c => c.Binary(storeType: "image"));
            DropForeignKey("dbo.GeneralBusinesses", "SuburbID", "dbo.Suburbs");
            DropIndex("dbo.GeneralBusinesses", new[] { "SuburbID" });
            DropColumn("dbo.GeneralBusinesses", "TelNo");
            DropColumn("dbo.GeneralBusinesses", "FaxNo");
            DropColumn("dbo.GeneralBusinesses", "SuburbID");
        }
    }
}
