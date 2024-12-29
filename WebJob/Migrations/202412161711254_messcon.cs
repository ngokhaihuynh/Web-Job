namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messcon : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false),
                        AdminID = c.String(nullable: false),
                        LastMessageTime = c.DateTime(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ConversationID);
            
            AddColumn("dbo.Message", "ConversationID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "SenderID", c => c.String(nullable: false));
            AlterColumn("dbo.Message", "Content", c => c.String(nullable: false));
            CreateIndex("dbo.Message", "ConversationID");
            AddForeignKey("dbo.Message", "ConversationID", "dbo.Conversations", "ConversationID", cascadeDelete: true);
            DropColumn("dbo.Message", "ReceiverID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "ReceiverID", c => c.String());
            DropForeignKey("dbo.Message", "ConversationID", "dbo.Conversations");
            DropIndex("dbo.Message", new[] { "ConversationID" });
            AlterColumn("dbo.Message", "Content", c => c.String());
            AlterColumn("dbo.Message", "SenderID", c => c.String());
            DropColumn("dbo.Message", "ConversationID");
            DropTable("dbo.Conversations");
        }
    }
}
