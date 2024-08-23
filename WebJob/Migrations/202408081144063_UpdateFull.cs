namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFull : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Job", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_Job", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.tb_Job", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tb_Job", "ModifiedBy", c => c.String());
            AddColumn("dbo.tb_CompanyImage", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_CompanyImage", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.tb_CompanyImage", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tb_CompanyImage", "ModifiedBy", c => c.String());
            AddColumn("dbo.tb_JobApplication_Detail", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_JobApplication_Detail", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.tb_JobApplication_Detail", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tb_JobApplication_Detail", "ModifiedBy", c => c.String());
            AlterColumn("dbo.tb_Article", "Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Article", "Content", c => c.String(nullable: false));
            DropColumn("dbo.tb_JobApplication_Detail", "ModifiedBy");
            DropColumn("dbo.tb_JobApplication_Detail", "ModifiedDate");
            DropColumn("dbo.tb_JobApplication_Detail", "CreatedDate");
            DropColumn("dbo.tb_JobApplication_Detail", "CreatedBy");
            DropColumn("dbo.tb_CompanyImage", "ModifiedBy");
            DropColumn("dbo.tb_CompanyImage", "ModifiedDate");
            DropColumn("dbo.tb_CompanyImage", "CreatedDate");
            DropColumn("dbo.tb_CompanyImage", "CreatedBy");
            DropColumn("dbo.tb_Job", "ModifiedBy");
            DropColumn("dbo.tb_Job", "ModifiedDate");
            DropColumn("dbo.tb_Job", "CreatedDate");
            DropColumn("dbo.tb_Job", "CreatedBy");
        }
    }
}
