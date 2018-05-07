namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.UserID);

            AddForeignKey("dbo.Workers", "UserID", "dbo.Users", "Id");

            
            CreateTable(
                "dbo.WorkerSkills",
                c => new
                    {
                        WSID = c.Int(nullable: false, identity: true),
                        WorkerID = c.String(nullable: false, maxLength:128),
                        SkillID = c.Int(nullable: false),
                        
                    })
                .PrimaryKey(t => t.WSID)
                .ForeignKey("dbo.Skills", t => t.SkillID, cascadeDelete: true)
                .Index(t => t.SkillID)
                .Index(t => t.WorkerID);

            AddForeignKey("dbo.WorkerSkills", "WorkerID", "dbo.Workers", "UserID");
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillID = c.Int(nullable: false, identity: true),
                        SkillDescription = c.String(),
                        Recom_Rate = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.SkillID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkerSkills", "Worker_WorkerID", "dbo.Workers");
            DropForeignKey("dbo.WorkerSkills", "SkillID", "dbo.Skills");
            DropForeignKey("dbo.Workers", "WorkerID", "dbo.AspNetUsers");
            DropIndex("dbo.WorkerSkills", new[] { "Worker_WorkerID" });
            DropIndex("dbo.WorkerSkills", new[] { "SkillID" });
            DropIndex("dbo.Workers", new[] { "WorkerID" });
            DropTable("dbo.Skills");
            DropTable("dbo.WorkerSkills");
            DropTable("dbo.Workers");
        }
    }
}
