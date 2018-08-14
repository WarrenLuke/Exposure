namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reviewer : DbMigration
    {
        public override void Up()
        {           
            
            AddColumn("dbo.Reviews", "Reviewee", c=>c.String(maxLength:128));
            CreateIndex("dbo.Reviews", "Reviewee");
            AddForeignKey("dbo.Reviews", "Reviewee", "dbo.AspNetUsers", "Id");
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        ReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.ReviewID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: false)
                .ForeignKey("dbo.Reviews", t => t.ReviewID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ReviewID);                         
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "Worker", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "Employer", c => c.String(maxLength: 128));
            AddColumn("dbo.Incidents", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            DropForeignKey("dbo.UserReviews", "ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.UserReviews", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.UserReviews", new[] { "ReviewID" });
            DropIndex("dbo.UserReviews", new[] { "UserID" });
            DropTable("dbo.UserReviews");
            RenameIndex(table: "dbo.Reviews", name: "IX_Reviewee", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Reviews", name: "Reviewee", newName: "ApplicationUser_Id");
            CreateIndex("dbo.Reviews", "ApplicationUser_Id1");
            CreateIndex("dbo.Reviews", "Worker");
            CreateIndex("dbo.Reviews", "Employer");
            CreateIndex("dbo.Incidents", "ApplicationUser_Id1");
            AddForeignKey("dbo.Reviews", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Incidents", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Reviews", "Worker", "dbo.Workers", "WorkerID");
            AddForeignKey("dbo.Reviews", "Employer", "dbo.Employers", "EmployerID");
        }
    }
}
