namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequirementsInternship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "Requirements", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "Requirements");
        }
    }
}
