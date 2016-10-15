namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThesisSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Theses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeName = c.String(nullable: false, maxLength: 250),
                        ThesisName = c.String(nullable: false, maxLength: 250),
                        Description = c.String(),
                        InternshipCategoryID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        Amount = c.Int(nullable: false),
                        CurrencyID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        ThesisTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyID, cascadeDelete: true)
                .ForeignKey("dbo.InternshipCategories", t => t.InternshipCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ThesisTypes", t => t.ThesisTypeID, cascadeDelete: true)
                .Index(t => t.CodeName)
                .Index(t => t.InternshipCategoryID)
                .Index(t => t.CompanyID)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CurrencyID)
                .Index(t => t.ThesisTypeID);
            
            CreateTable(
                "dbo.ThesisTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        CodeName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.CodeName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Theses", "ThesisTypeID", "dbo.ThesisTypes");
            DropForeignKey("dbo.Theses", "InternshipCategoryID", "dbo.InternshipCategories");
            DropForeignKey("dbo.Theses", "CurrencyID", "dbo.Currencies");
            DropForeignKey("dbo.Theses", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Theses", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ThesisTypes", new[] { "CodeName" });
            DropIndex("dbo.Theses", new[] { "ThesisTypeID" });
            DropIndex("dbo.Theses", new[] { "CurrencyID" });
            DropIndex("dbo.Theses", new[] { "ApplicationUserId" });
            DropIndex("dbo.Theses", new[] { "CompanyID" });
            DropIndex("dbo.Theses", new[] { "InternshipCategoryID" });
            DropIndex("dbo.Theses", new[] { "CodeName" });
            DropTable("dbo.ThesisTypes");
            DropTable("dbo.Theses");
        }
    }
}
