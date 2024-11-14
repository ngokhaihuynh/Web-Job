namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModifineApplicant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Applicant", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_Applicant", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.tb_Applicant", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tb_Applicant", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Applicant", "ModifiedBy");
            DropColumn("dbo.tb_Applicant", "ModifiedDate");
            DropColumn("dbo.tb_Applicant", "CreatedDate");
            DropColumn("dbo.tb_Applicant", "CreatedBy");
        }
    }
}
