namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateJobNơ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Job", "IsNow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Job", "IsNow");
        }
    }
}
