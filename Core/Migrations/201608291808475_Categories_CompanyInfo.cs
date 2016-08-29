namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories_CompanyInfo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Categories", newName: "InternshipCategories");
            CreateTable(
                "dbo.CompanyCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Companies", "YearFounded", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "PublicEmail", c => c.String());
            AddColumn("dbo.Companies", "ShortDescription", c => c.String());
            AddColumn("dbo.Companies", "LongDescription", c => c.String());
            AddColumn("dbo.Companies", "City", c => c.String());
            AddColumn("dbo.Companies", "Country", c => c.String());
            AddColumn("dbo.Companies", "CompanySize", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "CompanyCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Companies", "CompanyCategoryID");
            AddForeignKey("dbo.Companies", "CompanyCategoryID", "dbo.CompanyCategories", "ID", cascadeDelete: true);
            DropColumn("dbo.Companies", "Founded");
            DropColumn("dbo.Companies", "About");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "About", c => c.String());
            AddColumn("dbo.Companies", "Founded", c => c.Int(nullable: false));
            DropForeignKey("dbo.Companies", "CompanyCategoryID", "dbo.CompanyCategories");
            DropIndex("dbo.Companies", new[] { "CompanyCategoryID" });
            DropColumn("dbo.Companies", "CompanyCategoryID");
            DropColumn("dbo.Companies", "CompanySize");
            DropColumn("dbo.Companies", "Country");
            DropColumn("dbo.Companies", "City");
            DropColumn("dbo.Companies", "LongDescription");
            DropColumn("dbo.Companies", "ShortDescription");
            DropColumn("dbo.Companies", "PublicEmail");
            DropColumn("dbo.Companies", "YearFounded");
            DropTable("dbo.CompanyCategories");
            RenameTable(name: "dbo.InternshipCategories", newName: "Categories");
        }
    }
}
