namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatedb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Applicant", "FullName", c => c.String(maxLength: 255));
            AlterColumn("dbo.tb_Applicant", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.tb_Applicant", "PhoneNumber", c => c.String(maxLength: 50));
            AlterColumn("dbo.tb_Company", "CompanyName", c => c.String(maxLength: 255));
            AlterColumn("dbo.tb_Experience", "ExperienceLevel", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Experience", "ExperienceLevel", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.tb_Company", "CompanyName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.tb_Applicant", "PhoneNumber", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.tb_Applicant", "Email", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.tb_Applicant", "FullName", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
