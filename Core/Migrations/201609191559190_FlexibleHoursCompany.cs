namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlexibleHoursCompany : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Companies", new[] { "CompanyName" });
            AddColumn("dbo.Companies", "HasFlexibleHours", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "WorkHours", c => c.String(maxLength: 250));
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(maxLength: 100));
            CreateIndex("dbo.Companies", "CompanyName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", new[] { "CompanyName" });
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(maxLength: 150));
            DropColumn("dbo.Companies", "WorkHours");
            DropColumn("dbo.Companies", "HasFlexibleHours");
            CreateIndex("dbo.Companies", "CompanyName");
        }
    }
}
