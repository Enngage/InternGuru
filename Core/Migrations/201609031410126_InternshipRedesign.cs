namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipRedesign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "Amount", c => c.Double(nullable: false));
            AddColumn("dbo.Internships", "Currency", c => c.String());
            AlterColumn("dbo.Internships", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Internships", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Internships", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Internships", "Country", c => c.String(nullable: false));
            DropColumn("dbo.Internships", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Internships", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Internships", "Country", c => c.String());
            AlterColumn("dbo.Internships", "City", c => c.String());
            AlterColumn("dbo.Internships", "Description", c => c.String());
            AlterColumn("dbo.Internships", "Title", c => c.String());
            DropColumn("dbo.Internships", "Currency");
            DropColumn("dbo.Internships", "Amount");
        }
    }
}
