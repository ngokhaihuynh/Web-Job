namespace WebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdatebase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Adv",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                        Type = c.Int(nullable: false),
                        Link = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_Applicant",
                c => new
                    {
                        ApplicantID = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(nullable: false, maxLength: 50),
                        CoverLetter = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantID);
            
            CreateTable(
                "dbo.tb_JobApplication",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        ApplicantID = c.Int(nullable: false),
                        CVFilePath = c.String(maxLength: 500),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .ForeignKey("dbo.tb_Applicant", t => t.ApplicantID, cascadeDelete: true)
                .ForeignKey("dbo.tb_Job", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.tb_Job",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(nullable: false, maxLength: 255),
                        CompanyID = c.Int(),
                        SalaryID = c.Int(),
                        PositionID = c.Int(),
                        ExperienceID = c.Int(),
                        LocationID = c.Int(),
                        IndustryID = c.Int(),
                        LevelID = c.Int(),
                        JobCategoryID = c.Int(),
                        Alias = c.String(),
                        JobDescription = c.String(),
                        JobBenefits = c.String(),
                        JobRequirements = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.JobID)
                .ForeignKey("dbo.tb_Company", t => t.CompanyID)
                .ForeignKey("dbo.tb_Experience", t => t.ExperienceID)
                .ForeignKey("dbo.tb_Industry", t => t.IndustryID)
                .ForeignKey("dbo.tb_JobCategory", t => t.JobCategoryID)
                .ForeignKey("dbo.tb_Level", t => t.LevelID)
                .ForeignKey("dbo.tb_Location", t => t.LocationID)
                .ForeignKey("dbo.tb_Position", t => t.PositionID)
                .ForeignKey("dbo.tb_Salary", t => t.SalaryID)
                .ForeignKey("dbo.tb_Category", t => t.Category_Id)
                .Index(t => t.CompanyID)
                .Index(t => t.SalaryID)
                .Index(t => t.PositionID)
                .Index(t => t.ExperienceID)
                .Index(t => t.LocationID)
                .Index(t => t.IndustryID)
                .Index(t => t.LevelID)
                .Index(t => t.JobCategoryID)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.tb_Company",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 255),
                        CompanyDescription = c.String(),
                        CompanyEmail = c.String(),
                        CompanyAddress = c.String(),
                        Alias = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.tb_CompanyImage",
                c => new
                    {
                        CompanyImageID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        JobID = c.Int(nullable: false),
                        ImageURL = c.String(maxLength: 255),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyImageID)
                .ForeignKey("dbo.tb_Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.tb_Job", t => t.JobID, cascadeDelete: true)
                .Index(t => t.CompanyID)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.tb_Experience",
                c => new
                    {
                        ExperienceID = c.Int(nullable: false, identity: true),
                        ExperienceLevel = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ExperienceID);
            
            CreateTable(
                "dbo.tb_Industry",
                c => new
                    {
                        IndustryID = c.Int(nullable: false, identity: true),
                        IndustryName = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IndustryID);
            
            CreateTable(
                "dbo.tb_JobApplication_Detail",
                c => new
                    {
                        JobApplication_DetailId = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        CoverLetter = c.String(),
                        Note = c.String(),
                        JobApplication_JobApplicationID = c.Int(),
                    })
                .PrimaryKey(t => t.JobApplication_DetailId)
                .ForeignKey("dbo.tb_Job", t => t.JobID, cascadeDelete: true)
                .ForeignKey("dbo.tb_JobApplication", t => t.JobApplication_JobApplicationID)
                .Index(t => t.JobID)
                .Index(t => t.JobApplication_JobApplicationID);
            
            CreateTable(
                "dbo.tb_JobCategory",
                c => new
                    {
                        JobCategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 255),
                        Alias = c.String(maxLength: 255),
                        Icon = c.String(maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.JobCategoryID);
            
            CreateTable(
                "dbo.tb_JobSkill",
                c => new
                    {
                        JobSkillID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        SkillID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobSkillID)
                .ForeignKey("dbo.tb_Job", t => t.JobID, cascadeDelete: true)
                .ForeignKey("dbo.tb_Skill", t => t.SkillID, cascadeDelete: true)
                .Index(t => t.JobID)
                .Index(t => t.SkillID);
            
            CreateTable(
                "dbo.tb_Skill",
                c => new
                    {
                        SkillID = c.Int(nullable: false, identity: true),
                        SkillName = c.String(maxLength: 255),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.SkillID);
            
            CreateTable(
                "dbo.tb_Level",
                c => new
                    {
                        LevelID = c.Int(nullable: false, identity: true),
                        LevelName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.LevelID);
            
            CreateTable(
                "dbo.tb_Location",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.tb_Position",
                c => new
                    {
                        PositionID = c.Int(nullable: false, identity: true),
                        PositionName = c.String(maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID);
            
            CreateTable(
                "dbo.tb_Salary",
                c => new
                    {
                        SalaryID = c.Int(nullable: false, identity: true),
                        SalaryRange = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SalaryID);
            
            CreateTable(
                "dbo.tb_Article",
                c => new
                    {
                        ArticleID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Alias = c.String(maxLength: 255),
                        Content = c.String(nullable: false),
                        ImageURL = c.String(maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ArticleID);
            
            CreateTable(
                "dbo.tb_Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(),
                        Description = c.String(maxLength: 250),
                        Position = c.Int(nullable: false),
                        SeoTitle = c.String(),
                        SeoDescription = c.String(),
                        SeoKeywords = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_Contact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Website = c.String(maxLength: 150),
                        Email = c.String(nullable: false, maxLength: 150),
                        Message = c.String(nullable: false),
                        IsRead = c.Boolean(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_EmailSubscription",
                c => new
                    {
                        SubscriptionID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255),
                        JobCategoryID = c.Int(),
                        LocationID = c.Int(),
                        PositionID = c.Int(),
                        SalaryRange = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.SubscriptionID)
                .ForeignKey("dbo.tb_JobCategory", t => t.JobCategoryID)
                .ForeignKey("dbo.tb_Location", t => t.LocationID)
                .ForeignKey("dbo.tb_Position", t => t.PositionID)
                .Index(t => t.JobCategoryID)
                .Index(t => t.LocationID)
                .Index(t => t.PositionID);
            
            CreateTable(
                "dbo.tb_News",
                c => new
                    {
                        NewsID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 500),
                        Alias = c.String(),
                        Content = c.String(),
                        ImageURL = c.String(maxLength: 300),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.NewsID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.tb_Setting",
                c => new
                    {
                        SettingID = c.Int(nullable: false, identity: true),
                        SettingKey = c.String(maxLength: 255),
                        SettingValue = c.String(maxLength: 255),
                        Description = c.String(maxLength: 500),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.SettingID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Phone = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.tb_EmailSubscription", "PositionID", "dbo.tb_Position");
            DropForeignKey("dbo.tb_EmailSubscription", "LocationID", "dbo.tb_Location");
            DropForeignKey("dbo.tb_EmailSubscription", "JobCategoryID", "dbo.tb_JobCategory");
            DropForeignKey("dbo.tb_Job", "Category_Id", "dbo.tb_Category");
            DropForeignKey("dbo.tb_Job", "SalaryID", "dbo.tb_Salary");
            DropForeignKey("dbo.tb_Job", "PositionID", "dbo.tb_Position");
            DropForeignKey("dbo.tb_Job", "LocationID", "dbo.tb_Location");
            DropForeignKey("dbo.tb_Job", "LevelID", "dbo.tb_Level");
            DropForeignKey("dbo.tb_JobSkill", "SkillID", "dbo.tb_Skill");
            DropForeignKey("dbo.tb_JobSkill", "JobID", "dbo.tb_Job");
            DropForeignKey("dbo.tb_Job", "JobCategoryID", "dbo.tb_JobCategory");
            DropForeignKey("dbo.tb_JobApplication", "JobID", "dbo.tb_Job");
            DropForeignKey("dbo.tb_JobApplication_Detail", "JobApplication_JobApplicationID", "dbo.tb_JobApplication");
            DropForeignKey("dbo.tb_JobApplication_Detail", "JobID", "dbo.tb_Job");
            DropForeignKey("dbo.tb_Job", "IndustryID", "dbo.tb_Industry");
            DropForeignKey("dbo.tb_Job", "ExperienceID", "dbo.tb_Experience");
            DropForeignKey("dbo.tb_Job", "CompanyID", "dbo.tb_Company");
            DropForeignKey("dbo.tb_CompanyImage", "JobID", "dbo.tb_Job");
            DropForeignKey("dbo.tb_CompanyImage", "CompanyID", "dbo.tb_Company");
            DropForeignKey("dbo.tb_JobApplication", "ApplicantID", "dbo.tb_Applicant");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.tb_EmailSubscription", new[] { "PositionID" });
            DropIndex("dbo.tb_EmailSubscription", new[] { "LocationID" });
            DropIndex("dbo.tb_EmailSubscription", new[] { "JobCategoryID" });
            DropIndex("dbo.tb_JobSkill", new[] { "SkillID" });
            DropIndex("dbo.tb_JobSkill", new[] { "JobID" });
            DropIndex("dbo.tb_JobApplication_Detail", new[] { "JobApplication_JobApplicationID" });
            DropIndex("dbo.tb_JobApplication_Detail", new[] { "JobID" });
            DropIndex("dbo.tb_CompanyImage", new[] { "JobID" });
            DropIndex("dbo.tb_CompanyImage", new[] { "CompanyID" });
            DropIndex("dbo.tb_Job", new[] { "Category_Id" });
            DropIndex("dbo.tb_Job", new[] { "JobCategoryID" });
            DropIndex("dbo.tb_Job", new[] { "LevelID" });
            DropIndex("dbo.tb_Job", new[] { "IndustryID" });
            DropIndex("dbo.tb_Job", new[] { "LocationID" });
            DropIndex("dbo.tb_Job", new[] { "ExperienceID" });
            DropIndex("dbo.tb_Job", new[] { "PositionID" });
            DropIndex("dbo.tb_Job", new[] { "SalaryID" });
            DropIndex("dbo.tb_Job", new[] { "CompanyID" });
            DropIndex("dbo.tb_JobApplication", new[] { "ApplicantID" });
            DropIndex("dbo.tb_JobApplication", new[] { "JobID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.tb_Setting");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.tb_News");
            DropTable("dbo.tb_EmailSubscription");
            DropTable("dbo.tb_Contact");
            DropTable("dbo.tb_Category");
            DropTable("dbo.tb_Article");
            DropTable("dbo.tb_Salary");
            DropTable("dbo.tb_Position");
            DropTable("dbo.tb_Location");
            DropTable("dbo.tb_Level");
            DropTable("dbo.tb_Skill");
            DropTable("dbo.tb_JobSkill");
            DropTable("dbo.tb_JobCategory");
            DropTable("dbo.tb_JobApplication_Detail");
            DropTable("dbo.tb_Industry");
            DropTable("dbo.tb_Experience");
            DropTable("dbo.tb_CompanyImage");
            DropTable("dbo.tb_Company");
            DropTable("dbo.tb_Job");
            DropTable("dbo.tb_JobApplication");
            DropTable("dbo.tb_Applicant");
            DropTable("dbo.tb_Adv");
        }
    }
}
