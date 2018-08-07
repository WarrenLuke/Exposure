namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReviewIncident : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserIncidents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserIncidents", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserReviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserReviews", "ReviewID", "dbo.Reviews");
            DropIndex("dbo.UserIncidents", new[] { "UserId" });
            DropIndex("dbo.UserIncidents", new[] { "IncidentID" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.UserReviews", new[] { "UserId" });
            DropIndex("dbo.UserReviews", new[] { "ReviewID" });
            RenameColumn(table: "dbo.Incidents", name: "UserId", newName: "OffenderID");
            RenameIndex(table: "dbo.Incidents", name: "IX_UserId", newName: "IX_OffenderID");
            AddColumn("dbo.Incidents", "ReporterID", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "Employer", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "Worker", c => c.String(maxLength: 128));
            CreateIndex("dbo.Incidents", "ReporterID");
            CreateIndex("dbo.Reviews", "Employer");
            CreateIndex("dbo.Reviews", "Worker");
            AddForeignKey("dbo.Incidents", "ReporterID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Reviews", "Employer", "dbo.Employers", "EmployerID");
            AddForeignKey("dbo.Reviews", "Worker", "dbo.Workers", "WorkerID");
            DropColumn("dbo.Reviews", "UserId");
            DropTable("dbo.UserIncidents");
            DropTable("dbo.UserReviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReviewID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReviewID });
            
            CreateTable(
                "dbo.UserIncidents",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        IncidentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.IncidentID });
            
            AddColumn("dbo.Reviews", "UserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Reviews", "Worker", "dbo.Workers");
            DropForeignKey("dbo.Reviews", "Employer", "dbo.Employers");
            DropForeignKey("dbo.Incidents", "ReporterID", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "Worker" });
            DropIndex("dbo.Reviews", new[] { "Employer" });
            DropIndex("dbo.Incidents", new[] { "ReporterID" });
            DropColumn("dbo.Reviews", "Worker");
            DropColumn("dbo.Reviews", "Employer");
            DropColumn("dbo.Incidents", "ReporterID");
            RenameIndex(table: "dbo.Incidents", name: "IX_OffenderID", newName: "IX_UserId");
            RenameColumn(table: "dbo.Incidents", name: "OffenderID", newName: "UserId");
            CreateIndex("dbo.UserReviews", "ReviewID");
            CreateIndex("dbo.UserReviews", "UserId");
            CreateIndex("dbo.Reviews", "UserId");
            CreateIndex("dbo.UserIncidents", "IncidentID");
            CreateIndex("dbo.UserIncidents", "UserId");
            AddForeignKey("dbo.UserReviews", "ReviewID", "dbo.Reviews", "ReviewID", cascadeDelete: true);
            AddForeignKey("dbo.UserReviews", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserIncidents", "IncidentID", "dbo.Incidents", "IncidentID", cascadeDelete: true);
            AddForeignKey("dbo.UserIncidents", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
