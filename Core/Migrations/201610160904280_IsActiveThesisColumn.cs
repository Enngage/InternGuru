namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveThesisColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Theses", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Theses", "IsActive");
        }
    }
}
