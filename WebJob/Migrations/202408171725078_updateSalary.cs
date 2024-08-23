namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSalary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Salary", "SalaryMin", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.tb_Salary", "SalaryMax", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Salary", "SalaryMax");
            DropColumn("dbo.tb_Salary", "SalaryMin");
        }
    }
}
