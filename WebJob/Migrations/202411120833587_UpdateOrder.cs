namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_JobApplication", "CoverLetter", c => c.String());
            AddColumn("dbo.tb_JobApplication", "Note", c => c.String());
            AddColumn("dbo.tb_Order", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Email");
            DropColumn("dbo.tb_JobApplication", "Note");
            DropColumn("dbo.tb_JobApplication", "CoverLetter");
        }
    }
}
