namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipFlexibleHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "HasFlexibleHours", c => c.Boolean(nullable: false));
            AddColumn("dbo.Internships", "WorkingHours", c => c.String(maxLength: 250));
            AlterColumn("dbo.Internships", "Title", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Internships", "City", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Internships", "Country", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Internships", "Currency", c => c.String(maxLength: 50));
            AlterColumn("dbo.Internships", "AmountType", c => c.String(maxLength: 50));
            AlterColumn("dbo.Internships", "MaxDurationType", c => c.String(nullable: false));
            DropColumn("dbo.Companies", "HasFlexibleHours");
            DropColumn("dbo.Companies", "WorkHours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "WorkHours", c => c.String(maxLength: 250));
            AddColumn("dbo.Companies", "HasFlexibleHours", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Internships", "MaxDurationType", c => c.String());
            AlterColumn("dbo.Internships", "AmountType", c => c.String());
            AlterColumn("dbo.Internships", "Currency", c => c.String());
            AlterColumn("dbo.Internships", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.Internships", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Internships", "Title", c => c.String(nullable: false));
            DropColumn("dbo.Internships", "WorkingHours");
            DropColumn("dbo.Internships", "HasFlexibleHours");
        }
    }
}
