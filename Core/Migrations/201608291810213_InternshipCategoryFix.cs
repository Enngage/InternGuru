namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipCategoryFix : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Internships", name: "CategoryID", newName: "InternshipCategoryID");
            RenameIndex(table: "dbo.Internships", name: "IX_CategoryID", newName: "IX_InternshipCategoryID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Internships", name: "IX_InternshipCategoryID", newName: "IX_CategoryID");
            RenameColumn(table: "dbo.Internships", name: "InternshipCategoryID", newName: "CategoryID");
        }
    }
}
