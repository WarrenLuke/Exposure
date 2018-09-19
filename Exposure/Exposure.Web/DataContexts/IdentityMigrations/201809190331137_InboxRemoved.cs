namespace Exposure.Web.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InboxRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InboxApplicationUsers", "Inbox_InboxID", "dbo.Inboxes");
            DropForeignKey("dbo.InboxApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.InboxApplicationUsers", new[] { "Inbox_InboxID" });
            DropIndex("dbo.InboxApplicationUsers", new[] { "ApplicationUser_Id" });            
            
            DropTable("dbo.Inboxes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InboxApplicationUsers",
                c => new
                    {
                        Inbox_InboxID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Inbox_InboxID, t.ApplicationUser_Id });
            
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
            
            CreateIndex("dbo.InboxApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.InboxApplicationUsers", "Inbox_InboxID");
            AddForeignKey("dbo.InboxApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InboxApplicationUsers", "Inbox_InboxID", "dbo.Inboxes", "InboxID", cascadeDelete: true);
        }
    }
}
