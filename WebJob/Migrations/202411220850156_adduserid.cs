namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduserid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "UserId");
        }
    }
}
