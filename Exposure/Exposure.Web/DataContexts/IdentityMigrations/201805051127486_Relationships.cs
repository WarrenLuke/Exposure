namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relationships : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "SkillID", c => c.Int(nullable: false));
            AddColumn("dbo.JobApplications", "WorkerID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Jobs", "SkillID");
            CreateIndex("dbo.JobApplications", "WorkerID");
            AddForeignKey("dbo.Jobs", "SkillID", "dbo.Skills", "SkillID", cascadeDelete: true);
            AddForeignKey("dbo.JobApplications", "WorkerID", "dbo.Workers", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplications", "Worker_WorkerID", "dbo.Workers");
            DropForeignKey("dbo.Jobs", "SkillID", "dbo.Skills");
            DropIndex("dbo.JobApplications", new[] { "Worker_WorkerID" });
            DropIndex("dbo.Jobs", new[] { "SkillID" });
            DropColumn("dbo.JobApplications", "Worker_WorkerID");
            DropColumn("dbo.Jobs", "SkillID");
        }
    }
}
