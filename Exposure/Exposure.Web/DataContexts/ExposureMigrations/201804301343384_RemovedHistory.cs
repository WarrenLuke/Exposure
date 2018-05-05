namespace Exposure.Web.DataContexts.ExposureMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedHistory : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.JobHistories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobHistories",
                c => new
                    {
                        JobHistoryID = c.Int(nullable: false, identity: true),
                        ApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobHistoryID);
            
        }
    }
}
