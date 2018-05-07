namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employers",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        WorkNo = c.String(),
                        SuburbID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                
                .ForeignKey("dbo.Suburbs", t => t.SuburbID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.SuburbID);

            AddForeignKey("dbo.Employers", "UserId", "dbo.Users", "Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employers", "SuburbID", "dbo.Suburbs");
            DropForeignKey("dbo.Employers", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Employers", new[] { "SuburbID" });
            DropIndex("dbo.Employers", new[] { "UserID" });
            DropTable("dbo.Employers");
        }
    }
}
