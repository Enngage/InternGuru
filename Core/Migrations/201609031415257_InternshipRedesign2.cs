namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipRedesign2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "DurationInDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "DurationInDays");
        }
    }
}
