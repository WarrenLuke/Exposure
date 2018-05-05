namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldTypes : DbMigration
    {
        public override void Up()
        {
            
            DropForeignKey("dbo.Reviews", "JobApplication_JobApplicationID", "dbo.JobApplications");
            
            DropIndex("dbo.Reviews", new[] { "JobApplication_JobApplicationID" });
           
            RenameColumn(table: "dbo.AspNetUsers", name: "Suburb_SuburbID", newName: "SuburbID");
            RenameColumn(table: "dbo.UserIncidents", name: "Id", newName: "UserId");
            RenameColumn(table: "dbo.Reviews", name: "JobApplication_JobApplicationID", newName: "JobApplicationID");
            RenameIndex(table: "dbo.UserIncidents", name: "IX_Id", newName: "IX_UserId");
            AlterColumn("dbo.Reviews", "JobApplicationID", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "SuburbID");
            CreateIndex("dbo.Reviews", "JobApplicationID");
            AddForeignKey("dbo.AspNetUsers", "SuburbID", "dbo.Suburbs", "SuburbID", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications", "JobApplicationID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.AspNetUsers", "SuburbID", "dbo.Suburbs");
            DropIndex("dbo.Reviews", new[] { "JobApplicationID" });
            AlterColumn("dbo.Reviews", "JobApplicationID", c => c.Int());
            
            RenameIndex(table: "dbo.UserIncidents", name: "IX_UserId", newName: "IX_Id");
            RenameColumn(table: "dbo.Reviews", name: "JobApplicationID", newName: "JobApplication_JobApplicationID");
            RenameColumn(table: "dbo.UserIncidents", name: "UserId", newName: "Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "SuburbID", newName: "Suburb_SuburbID");
            AddColumn("dbo.AspNetUsers", "SuburbID", c => c.String(nullable: false));
            CreateIndex("dbo.Reviews", "JobApplication_JobApplicationID");
            CreateIndex("dbo.AspNetUsers", "Suburb_SuburbID");
            AddForeignKey("dbo.Reviews", "JobApplication_JobApplicationID", "dbo.JobApplications", "JobApplicationID");
            AddForeignKey("dbo.AspNetUsers", "Suburb_SuburbID", "dbo.Suburbs", "SuburbID");
        }
    }
}
