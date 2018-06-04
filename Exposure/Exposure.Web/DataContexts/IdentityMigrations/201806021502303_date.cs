namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: true, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Jobs", "EndDate", c => c.DateTime(nullable: true, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
           
        }
    }
}
