namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneralBus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneralBusinesses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyLogo = c.Binary(storeType: "image"),
                        Logo = c.Binary(storeType: "image"),
                        CompanyName = c.String(nullable: false),
                        Slogan = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GeneralBusinesses");
        }
    }
}
