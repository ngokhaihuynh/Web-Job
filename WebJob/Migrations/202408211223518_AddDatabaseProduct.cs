namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatabaseProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_CategoryProduct",
                c => new
                    {
                        CateProId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(),
                        Description = c.String(maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.CateProId);
            
            CreateTable(
                "dbo.tb_Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 500),
                        Description = c.String(),
                        Alias = c.String(),
                        ProductCode = c.String(),
                        CateProId = c.Int(nullable: false),
                        Images = c.String(maxLength: 300),
                        Detail = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceSale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.tb_CategoryProduct", t => t.CateProId, cascadeDelete: true)
                .Index(t => t.CateProId);
            
            CreateTable(
                "dbo.tb_ProductImage",
                c => new
                    {
                        ProductImgID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Image = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductImgID)
                .ForeignKey("dbo.tb_Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.tb_OrderDetail",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.tb_Order", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.tb_Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.tb_Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        CustomerName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        TotalAmount = c.String(),
                        Quantyti = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_OrderDetail", "ProductID", "dbo.tb_Product");
            DropForeignKey("dbo.tb_OrderDetail", "OrderID", "dbo.tb_Order");
            DropForeignKey("dbo.tb_ProductImage", "ProductID", "dbo.tb_Product");
            DropForeignKey("dbo.tb_Product", "CateProId", "dbo.tb_CategoryProduct");
            DropIndex("dbo.tb_OrderDetail", new[] { "ProductID" });
            DropIndex("dbo.tb_OrderDetail", new[] { "OrderID" });
            DropIndex("dbo.tb_ProductImage", new[] { "ProductID" });
            DropIndex("dbo.tb_Product", new[] { "CateProId" });
            DropTable("dbo.tb_Order");
            DropTable("dbo.tb_OrderDetail");
            DropTable("dbo.tb_ProductImage");
            DropTable("dbo.tb_Product");
            DropTable("dbo.tb_CategoryProduct");
        }
    }
}
