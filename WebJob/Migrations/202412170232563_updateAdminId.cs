namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAdminId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Conversations", "AdminID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Conversations", "AdminID", c => c.String(nullable: false));
        }
    }
}
