namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityOptimization : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 150));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 150));
            AlterColumn("dbo.CompanyCategories", "CodeName", c => c.String(maxLength: 100));
            AlterColumn("dbo.CompanyCategories", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.CompanySizes", "CompanySizeName", c => c.String(maxLength: 100));
            AlterColumn("dbo.CompanySizes", "CodeName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Countries", "CountryCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.Countries", "CountryName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Countries", "CodeName", c => c.String(maxLength: 50));
            AlterColumn("dbo.InternshipAmountTypes", "AmountTypeName", c => c.String(maxLength: 50));
            AlterColumn("dbo.InternshipAmountTypes", "CodeName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Currencies", "CurrencyName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Currencies", "CodeName", c => c.String(maxLength: 50));
            AlterColumn("dbo.InternshipCategories", "CodeName", c => c.String(maxLength: 50));
            AlterColumn("dbo.InternshipCategories", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.InternshipDurationTypes", "DurationName", c => c.String(maxLength: 50));
            AlterColumn("dbo.InternshipDurationTypes", "CodeName", c => c.String(maxLength: 50));
            CreateIndex("dbo.CompanyCategories", "CodeName");
            CreateIndex("dbo.CompanySizes", "CodeName");
            CreateIndex("dbo.Countries", "CodeName");
            CreateIndex("dbo.InternshipAmountTypes", "CodeName");
            CreateIndex("dbo.Currencies", "CodeName");
            CreateIndex("dbo.InternshipCategories", "CodeName");
            CreateIndex("dbo.InternshipDurationTypes", "CodeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InternshipDurationTypes", new[] { "CodeName" });
            DropIndex("dbo.InternshipCategories", new[] { "CodeName" });
            DropIndex("dbo.Currencies", new[] { "CodeName" });
            DropIndex("dbo.InternshipAmountTypes", new[] { "CodeName" });
            DropIndex("dbo.Countries", new[] { "CodeName" });
            DropIndex("dbo.CompanySizes", new[] { "CodeName" });
            DropIndex("dbo.CompanyCategories", new[] { "CodeName" });
            AlterColumn("dbo.InternshipDurationTypes", "CodeName", c => c.String());
            AlterColumn("dbo.InternshipDurationTypes", "DurationName", c => c.String());
            AlterColumn("dbo.InternshipCategories", "Name", c => c.String());
            AlterColumn("dbo.InternshipCategories", "CodeName", c => c.String());
            AlterColumn("dbo.Currencies", "CodeName", c => c.String());
            AlterColumn("dbo.Currencies", "CurrencyName", c => c.String());
            AlterColumn("dbo.InternshipAmountTypes", "CodeName", c => c.String());
            AlterColumn("dbo.InternshipAmountTypes", "AmountTypeName", c => c.String());
            AlterColumn("dbo.Countries", "CodeName", c => c.String());
            AlterColumn("dbo.Countries", "CountryName", c => c.String());
            AlterColumn("dbo.Countries", "CountryCode", c => c.String());
            AlterColumn("dbo.CompanySizes", "CodeName", c => c.String());
            AlterColumn("dbo.CompanySizes", "CompanySizeName", c => c.String());
            AlterColumn("dbo.CompanyCategories", "Name", c => c.String());
            AlterColumn("dbo.CompanyCategories", "CodeName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
        }
    }
}
