namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Adv", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_JobApplication", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_Company", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_JobCategory", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_Article", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_Category", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_Contact", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_EmailSubscription", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_News", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.tb_Setting", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Setting", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_News", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_EmailSubscription", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_Contact", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_Category", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_Article", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_JobCategory", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_Company", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_JobApplication", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_Adv", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
