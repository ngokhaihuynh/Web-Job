namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTypeSalary : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Salary", "SalaryMin", c => c.Int(nullable: false));
            AlterColumn("dbo.tb_Salary", "SalaryMax", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Salary", "SalaryMax", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.tb_Salary", "SalaryMin", c => c.Decimal(nullable: false, storeType: "money"));
        }
    }
}
