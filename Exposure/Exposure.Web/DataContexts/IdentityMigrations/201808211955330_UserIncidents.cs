namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIncidents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incidents", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.Incidents", "ReporterID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "Job_JobID", "dbo.Jobs");
            DropIndex("dbo.Incidents", new[] { "ReporterID" });
            DropIndex("dbo.Incidents", new[] { "JobApplicationID" });
            DropIndex("dbo.Incidents", new[] { "Job_JobID" });          
            RenameColumn(table: "dbo.Incidents", name: "Job_JobID", newName: "JobID");
            CreateTable(
                "dbo.UserIncidents",
                c => new
                    {
                        IncidentID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        UIID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.UIID)
                .ForeignKey("dbo.Incidents", t => t.IncidentID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.IncidentID)
                .Index(t => t.UserID);
            
            AddColumn("dbo.Incidents", "ReportDate", c => c.DateTime());
            AlterColumn("dbo.Incidents", "JobID", c => c.Int(nullable: false));
            CreateIndex("dbo.Incidents", "JobID");
            AddForeignKey("dbo.Incidents", "JobID", "dbo.Jobs", "JobID", cascadeDelete: true);
            DropColumn("dbo.Incidents", "ReporterID");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incidents", "JobApplicationID", c => c.Int(nullable: false));
            AddColumn("dbo.Incidents", "ReporterID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Incidents", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.UserIncidents", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserIncidents", "IncidentID", "dbo.Incidents");
            DropIndex("dbo.UserIncidents", new[] { "UserID" });
            DropIndex("dbo.UserIncidents", new[] { "IncidentID" });
            DropIndex("dbo.Incidents", new[] { "JobID" });
            AlterColumn("dbo.Incidents", "JobID", c => c.Int());
            DropColumn("dbo.Incidents", "ReportDate");
            DropTable("dbo.UserIncidents");
            RenameColumn(table: "dbo.Incidents", name: "JobID", newName: "Job_JobID");
            RenameColumn(table: "dbo.Incidents", name: "OffenderID", newName: "ApplicationUser_Id");
            AddColumn("dbo.Incidents", "OffenderID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Incidents", "ApplicationUser_Id");
            CreateIndex("dbo.Incidents", "Job_JobID");
            CreateIndex("dbo.Incidents", "JobApplicationID");
            CreateIndex("dbo.Incidents", "ReporterID");
            AddForeignKey("dbo.Incidents", "Job_JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.Incidents", "ReporterID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Incidents", "JobApplicationID", "dbo.JobApplications", "JobApplicationID", cascadeDelete: true);
        }
    }
}
