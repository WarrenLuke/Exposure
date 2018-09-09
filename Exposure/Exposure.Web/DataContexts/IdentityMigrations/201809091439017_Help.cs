namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Help : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Helps",
                c => new
                    {
                        HelpID = c.Int(nullable: false, identity: true),
                        HelpQuestion = c.String(),
                        HelpAnswer = c.String(),
                    })
                .PrimaryKey(t => t.HelpID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Helps");
        }
    }
}
