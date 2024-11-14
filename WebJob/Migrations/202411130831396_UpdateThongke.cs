namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThongke : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ThongKes", newName: "tb_ThongKe");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tb_ThongKe", newName: "ThongKes");
        }
    }
}
