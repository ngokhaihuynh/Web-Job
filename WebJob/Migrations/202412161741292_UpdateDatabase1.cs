namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Message", "ReceiverID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "ReceiverID", c => c.String());
        }
    }
}
