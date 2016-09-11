namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "IsActive");
        }
    }
}
