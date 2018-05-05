namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorChanges : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Employers", name: "UserID", newName: "EmployerID");
            RenameIndex(table: "dbo.Employers", name: "IX_UserID", newName: "IX_EmployerID");
            AddColumn("dbo.Jobs", "AgreedRate", c => c.Single(nullable: false));
            AlterColumn("dbo.Jobs", "EmployerID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Jobs", "Rate", c => c.Double(nullable: false));
            CreateIndex("dbo.Jobs", "EmployerID");
            AddForeignKey("dbo.Jobs", "EmployerID", "dbo.Employers", "EmployerID");
            DropColumn("dbo.Jobs", "HourlyRate");


        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "HourlyRate", c => c.Double(nullable: false));
            DropForeignKey("dbo.Jobs", "EmployerID", "dbo.Employers");
            DropIndex("dbo.Jobs", new[] { "EmployerID" });
            AlterColumn("dbo.Jobs", "Rate", c => c.Single(nullable: false));
            AlterColumn("dbo.Jobs", "EmployerID", c => c.Int(nullable: false));
            DropColumn("dbo.Jobs", "AgreedRate");
            RenameIndex(table: "dbo.Employers", name: "IX_EmployerID", newName: "IX_UserID");
            RenameColumn(table: "dbo.Employers", name: "EmployerID", newName: "UserID");
        }
    }
}
