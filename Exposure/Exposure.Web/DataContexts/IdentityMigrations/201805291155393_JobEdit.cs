namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "DateAdvertised", c => c.DateTime(nullable: true, defaultValue:null));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "DateAdvertised");
        }
    }
}
