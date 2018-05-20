namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkerFK : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkerSkills", new[] { "Worker_WorkerID" });
            DropColumn("dbo.WorkerSkills", "WorkerID");
            RenameColumn(table: "dbo.WorkerSkills", name: "Worker_WorkerID", newName: "WorkerID");
            AlterColumn("dbo.WorkerSkills", "WorkerID", c => c.String(maxLength: 128));
            CreateIndex("dbo.WorkerSkills", "WorkerID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkerSkills", new[] { "WorkerID" });
            AlterColumn("dbo.WorkerSkills", "WorkerID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.WorkerSkills", name: "WorkerID", newName: "Worker_WorkerID");
            AddColumn("dbo.WorkerSkills", "WorkerID", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkerSkills", "Worker_WorkerID");
        }
    }
}
