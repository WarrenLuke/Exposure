namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCompletionDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Jobs", "CompletionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "CompletionDate", c => c.DateTime());
        }
    }
}
