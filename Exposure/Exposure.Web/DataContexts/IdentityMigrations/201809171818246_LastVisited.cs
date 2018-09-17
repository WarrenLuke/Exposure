namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastVisited : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastVisited", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastVisited");
        }
    }
}
