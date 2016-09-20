namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipCategoryCodeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InternshipCategories", "CodeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InternshipCategories", "CodeName");
        }
    }
}
