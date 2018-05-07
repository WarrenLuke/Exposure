namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorUpdate : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.Users", "Incident_IncidentID", c => c.Int());
            AddColumn("dbo.Users", "Review_ReviewID", c => c.Int());
            AddColumn("dbo.Incidents", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Incidents", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "Reviewee", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Reviews", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AlterColumn("dbo.Incidents", "Offender", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.JobApplications", "WorkerID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Users", "Incident_IncidentID");
            CreateIndex("dbo.Users", "Review_ReviewID");
            CreateIndex("dbo.Incidents", "Offender");
            CreateIndex("dbo.Incidents", "ApplicationUser_Id");
            CreateIndex("dbo.Incidents", "ApplicationUser_Id1");
            CreateIndex("dbo.Reviews", "Reviewee");
            CreateIndex("dbo.Reviews", "ApplicationUser_Id");
            CreateIndex("dbo.Reviews", "ApplicationUser_Id1");
            AddForeignKey("dbo.Incidents", "Offender", "dbo.Users", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Users", "Incident_IncidentID", "dbo.Incidents", "IncidentID");
            AddForeignKey("dbo.Reviews", "Reviewee", "dbo.Users", "Id");
            AddForeignKey("dbo.Users", "Review_ReviewID", "dbo.Reviews", "ReviewID");
            AddForeignKey("dbo.Incidents", "ApplicationUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Incidents", "ApplicationUser_Id1", "dbo.Users", "Id");
            AddForeignKey("dbo.Reviews", "ApplicationUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Reviews", "ApplicationUser_Id1", "dbo.Users", "Id");
            DropColumn("dbo.Incidents", "Reporter");
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReviewApplicationUsers",
                c => new
                    {
                        Review_ReviewID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Review_ReviewID, t.ApplicationUser_Id });
            
            CreateTable(
                "dbo.IncidentApplicationUsers",
                c => new
                    {
                        Incident_IncidentID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Incident_IncidentID, t.ApplicationUser_Id });
            
            AddColumn("dbo.Incidents", "Reporter", c => c.String(nullable: false));
            DropForeignKey("dbo.Reviews", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Review_ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.Reviews", "Reviewee", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Incident_IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "Offender", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Reviews", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reviews", new[] { "Reviewee" });
            DropIndex("dbo.JobApplications", new[] { "WorkerID" });
            DropIndex("dbo.Incidents", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Incidents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Incidents", new[] { "Offender" });
            DropIndex("dbo.AspNetUsers", new[] { "Review_ReviewID" });
            DropIndex("dbo.AspNetUsers", new[] { "Incident_IncidentID" });
            AlterColumn("dbo.JobApplications", "WorkerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Incidents", "Offender", c => c.String(nullable: false));
            DropColumn("dbo.Reviews", "ApplicationUser_Id1");
            DropColumn("dbo.Reviews", "ApplicationUser_Id");
            DropColumn("dbo.Reviews", "Reviewee");
            DropColumn("dbo.Incidents", "ApplicationUser_Id1");
            DropColumn("dbo.Incidents", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "Review_ReviewID");
            DropColumn("dbo.AspNetUsers", "Incident_IncidentID");
            RenameColumn(table: "dbo.JobApplications", name: "WorkerID", newName: "Worker_WorkerID");
            AddColumn("dbo.JobApplications", "WorkerID", c => c.Int(nullable: false));
            CreateIndex("dbo.ReviewApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.ReviewApplicationUsers", "Review_ReviewID");
            CreateIndex("dbo.IncidentApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.IncidentApplicationUsers", "Incident_IncidentID");
            CreateIndex("dbo.JobApplications", "Worker_WorkerID");
            AddForeignKey("dbo.ReviewApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReviewApplicationUsers", "Review_ReviewID", "dbo.Reviews", "ReviewID", cascadeDelete: true);
            AddForeignKey("dbo.IncidentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IncidentApplicationUsers", "Incident_IncidentID", "dbo.Incidents", "IncidentID", cascadeDelete: true);
        }
    }
}
