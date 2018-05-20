namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WSCompositeKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkerSkills", "WorkerID", "dbo.Workers");
            DropIndex("dbo.WorkerSkills", new[] { "WorkerID" });
            DropPrimaryKey("dbo.WorkerSkills");
            AlterColumn("dbo.WorkerSkills", "WorkerID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.WorkerSkills", new[] { "WorkerID", "SkillID" });
            CreateIndex("dbo.WorkerSkills", "WorkerID");
            AddForeignKey("dbo.WorkerSkills", "WorkerID", "dbo.Workers", "WorkerID", cascadeDelete: true);
            DropColumn("dbo.WorkerSkills", "WSID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkerSkills", "WSID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.WorkerSkills", "WorkerID", "dbo.Workers");
            DropIndex("dbo.WorkerSkills", new[] { "WorkerID" });
            DropPrimaryKey("dbo.WorkerSkills");
            AlterColumn("dbo.WorkerSkills", "WorkerID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.WorkerSkills", "WSID");
            CreateIndex("dbo.WorkerSkills", "WorkerID");
            AddForeignKey("dbo.WorkerSkills", "WorkerID", "dbo.Workers", "WorkerID");
        }
    }
}
