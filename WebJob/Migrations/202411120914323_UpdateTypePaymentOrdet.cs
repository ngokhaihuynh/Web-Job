namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTypePaymentOrdet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.tb_Order", "TypePayment", c => c.Int(nullable: false));
            DropColumn("dbo.tb_Order", "Quantyti");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "Quantyti", c => c.Int(nullable: false));
            DropColumn("dbo.tb_Order", "TypePayment");
            DropColumn("dbo.tb_Order", "Quantity");
        }
    }
}
