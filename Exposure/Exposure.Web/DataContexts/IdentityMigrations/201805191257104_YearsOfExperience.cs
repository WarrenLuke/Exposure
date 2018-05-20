namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YearsOfExperience : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerSkills", "YearsOfExperience", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkerSkills", "YearsOfExperience");
        }
    }
}
