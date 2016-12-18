namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipRequirementsChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Internships", "Requirements", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Internships", "Requirements", c => c.String(nullable: false));
        }
    }
}
