namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeNameToInternship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "CodeName", c => c.String(nullable: false, maxLength: 250));
            CreateIndex("dbo.Internships", "CodeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Internships", new[] { "CodeName" });
            DropColumn("dbo.Internships", "CodeName");
        }
    }
}
