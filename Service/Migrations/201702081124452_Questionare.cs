namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Questionare : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questionares",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeName = c.String(nullable: false, maxLength: 50),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        CompanyID = c.Int(nullable: false),
                        QuestionareName = c.String(nullable: false),
                        QuestionareDefinitionXml = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CodeName)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CompanyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questionares", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Questionares", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Questionares", new[] { "CompanyID" });
            DropIndex("dbo.Questionares", new[] { "ApplicationUserId" });
            DropIndex("dbo.Questionares", new[] { "CodeName" });
            DropTable("dbo.Questionares");
        }
    }
}
