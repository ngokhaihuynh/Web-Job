namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateApplicant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Applicant", "CVFilePath", c => c.String());
            DropColumn("dbo.tb_JobApplication", "CVFilePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_JobApplication", "CVFilePath", c => c.String(maxLength: 500));
            DropColumn("dbo.tb_Applicant", "CVFilePath");
        }
    }
}
