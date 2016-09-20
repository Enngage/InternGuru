namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyCategoryCodeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyCategories", "CodeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyCategories", "CodeName");
        }
    }
}
