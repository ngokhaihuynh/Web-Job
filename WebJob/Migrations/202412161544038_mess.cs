namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        SenderID = c.String(),
                        ReceiverID = c.String(),
                        Content = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Message");
        }
    }
}
