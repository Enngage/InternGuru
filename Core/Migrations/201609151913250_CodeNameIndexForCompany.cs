namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeNameIndexForCompany : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(maxLength: 150));
            AlterColumn("dbo.Companies", "CodeName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "Address", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "City", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "Country", c => c.String(maxLength: 100));
            AlterColumn("dbo.Companies", "Web", c => c.String(maxLength: 250));
            AlterColumn("dbo.Companies", "Twitter", c => c.String(maxLength: 250));
            AlterColumn("dbo.Companies", "LinkedIn", c => c.String(maxLength: 250));
            AlterColumn("dbo.Companies", "Facebook", c => c.String(maxLength: 250));
            CreateIndex("dbo.Companies", "CodeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", new[] { "CodeName" });
            AlterColumn("dbo.Companies", "Facebook", c => c.String());
            AlterColumn("dbo.Companies", "LinkedIn", c => c.String());
            AlterColumn("dbo.Companies", "Twitter", c => c.String());
            AlterColumn("dbo.Companies", "Web", c => c.String());
            AlterColumn("dbo.Companies", "Country", c => c.String());
            AlterColumn("dbo.Companies", "City", c => c.String());
            AlterColumn("dbo.Companies", "Address", c => c.String());
            AlterColumn("dbo.Companies", "CodeName", c => c.String());
            AlterColumn("dbo.Companies", "CompanyName", c => c.String());
        }
    }
}
