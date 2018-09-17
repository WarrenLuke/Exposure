namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inbox : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inboxes",
                c => new
                    {
                        InboxID = c.Int(nullable: false, identity: true),
                        Sender = c.String(),
                        Receiver = c.String(),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InboxID);
            
            CreateTable(
                "dbo.UserInboxes",
                c => new
                    {
                        InID = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.InID, t.UserId })
                .ForeignKey("dbo.Inboxes", t => t.InID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.InID)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InboxApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InboxApplicationUsers", "Inbox_InboxID", "dbo.Inboxes");
            DropIndex("dbo.InboxApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.InboxApplicationUsers", new[] { "Inbox_InboxID" });
            DropTable("dbo.InboxApplicationUsers");
            DropTable("dbo.Inboxes");
        }
    }
}
