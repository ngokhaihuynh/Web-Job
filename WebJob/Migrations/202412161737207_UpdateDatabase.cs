namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "ReceiverID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "ReceiverID");
        }
    }
}
