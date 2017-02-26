namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCodeNames : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Internships", new[] { "CodeName" });
            DropIndex("dbo.Theses", new[] { "CodeName" });
            DropIndex("dbo.Questionnaires", new[] { "CodeName" });
            AlterColumn("dbo.Internships", "CodeName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Theses", "CodeName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Questionnaires", "CodeName", c => c.String(nullable: false, maxLength: 250));
            CreateIndex("dbo.Internships", "CodeName");
            CreateIndex("dbo.Theses", "CodeName");
            CreateIndex("dbo.Questionnaires", "CodeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Questionnaires", new[] { "CodeName" });
            DropIndex("dbo.Theses", new[] { "CodeName" });
            DropIndex("dbo.Internships", new[] { "CodeName" });
            AlterColumn("dbo.Questionnaires", "CodeName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Theses", "CodeName", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Internships", "CodeName", c => c.String(nullable: false, maxLength: 250));
            CreateIndex("dbo.Questionnaires", "CodeName");
            CreateIndex("dbo.Theses", "CodeName");
            CreateIndex("dbo.Internships", "CodeName");
        }
    }
}
