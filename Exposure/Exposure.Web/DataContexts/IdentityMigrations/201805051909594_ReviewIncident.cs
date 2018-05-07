namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewIncident : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "dbo.UserIncidents",
                c => new
                    {
                        IncidentID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.IncidentID, t.UserId })
                .ForeignKey("dbo.Incidents", t => t.IncidentID, cascadeDelete: false)
                
                .Index(t => t.IncidentID)
                .Index(t => t.UserId);

            AddForeignKey("dbo.UserIncidents", "UserId", "dbo.Users", "Id");
            
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ReviewID, t.UserID })
                .ForeignKey("dbo.Reviews", t => t.ReviewID, cascadeDelete: false)
                
                .Index(t => t.ReviewID)
                .Index(t => t.UserID);

            AddForeignKey("dbo.UserReviews", "UserID", "dbo.Users", "Id");

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        UserReviewID = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        ReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserReviewID);
            
            CreateTable(
                "dbo.UserIncidents",
                c => new
                    {
                        UserIncID = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        IncidentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserIncID);
            
            DropForeignKey("dbo.ReviewApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReviewApplicationUsers", "Review_ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.IncidentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.IncidentApplicationUsers", "Incident_IncidentID", "dbo.Incidents");
            DropIndex("dbo.ReviewApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ReviewApplicationUsers", new[] { "Review_ReviewID" });
            DropIndex("dbo.IncidentApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IncidentApplicationUsers", new[] { "Incident_IncidentID" });
            DropTable("dbo.ReviewApplicationUsers");
            DropTable("dbo.IncidentApplicationUsers");
            CreateIndex("dbo.UserReviews", "ReviewID");
            CreateIndex("dbo.UserReviews", "UserId");
            CreateIndex("dbo.UserIncidents", "IncidentID");
            CreateIndex("dbo.UserIncidents", "UserId");
            AddForeignKey("dbo.UserReviews", "ReviewID", "dbo.Reviews", "ReviewID", cascadeDelete: true);
            AddForeignKey("dbo.UserReviews", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserIncidents", "IncidentID", "dbo.Incidents", "IncidentID", cascadeDelete: true);
            AddForeignKey("dbo.UserIncidents", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
