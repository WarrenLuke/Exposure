namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecomRateCurrency : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Skills", "Recom_Rate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Skills", "Recom_Rate", c => c.Single(nullable: false));
        }
    }
}
