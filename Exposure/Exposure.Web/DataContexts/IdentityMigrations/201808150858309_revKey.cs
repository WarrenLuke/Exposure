namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserReviews", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.UserReviews", new[] { "UserID" });
            DropPrimaryKey("dbo.UserReviews");
            AddColumn("dbo.UserReviews", "URID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.UserReviews", "UserID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.UserReviews", "URID");
            CreateIndex("dbo.UserReviews", "UserID");
            AddForeignKey("dbo.UserReviews", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserReviews", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.UserReviews", new[] { "UserID" });
            DropPrimaryKey("dbo.UserReviews");
            AlterColumn("dbo.UserReviews", "UserID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.UserReviews", "URID");
            AddPrimaryKey("dbo.UserReviews", new[] { "UserID", "ReviewID" });
            CreateIndex("dbo.UserReviews", "UserID");
            AddForeignKey("dbo.UserReviews", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
