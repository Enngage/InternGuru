namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minAndMaxDurationInternship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "MinDurationInMonths", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "MinDurationInDays", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "MinDurationInWeeks", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "MaxDurationInMonths", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "MaxDurationInDays", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "MaxDurationInWeeks", c => c.Int(nullable: false));
            DropColumn("dbo.Internships", "DurationInMonths");
            DropColumn("dbo.Internships", "DurationInDays");
            DropColumn("dbo.Internships", "DurationInWeeks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Internships", "DurationInWeeks", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "DurationInDays", c => c.Int(nullable: false));
            AddColumn("dbo.Internships", "DurationInMonths", c => c.Int(nullable: false));
            DropColumn("dbo.Internships", "MaxDurationInWeeks");
            DropColumn("dbo.Internships", "MaxDurationInDays");
            DropColumn("dbo.Internships", "MaxDurationInMonths");
            DropColumn("dbo.Internships", "MinDurationInWeeks");
            DropColumn("dbo.Internships", "MinDurationInDays");
            DropColumn("dbo.Internships", "MinDurationInMonths");
        }
    }
}
