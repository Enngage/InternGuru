namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeName = c.String(maxLength: 50),
                        LanguageName = c.String(maxLength: 50),
                        IconClass = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.CodeName);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Languages", new[] { "CodeName" });
            DropTable("dbo.Languages");
        }
    }
}
