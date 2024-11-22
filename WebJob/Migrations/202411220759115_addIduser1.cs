namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIduser1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tb_Order", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "UserId", c => c.String());
        }
    }
}
