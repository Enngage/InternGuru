namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questionares", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Questionares", "CompanyID", "dbo.Companies");
            DropIndex("dbo.Questionares", new[] { "CodeName" });
            DropIndex("dbo.Questionares", new[] { "ApplicationUserId" });
            DropIndex("dbo.Questionares", new[] { "CompanyID" });
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeName = c.String(nullable: false, maxLength: 50),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        CompanyID = c.Int(nullable: false),
                        QuestionnaireName = c.String(nullable: false),
                        QuestionnaireXml = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CodeName)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CompanyID);
            
            DropTable("dbo.Questionares");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Questionnaires", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Questionnaires", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Questionnaires", new[] { "CompanyID" });
            DropIndex("dbo.Questionnaires", new[] { "ApplicationUserId" });
            DropIndex("dbo.Questionnaires", new[] { "CodeName" });
            DropTable("dbo.Questionnaires");
            CreateIndex("dbo.Questionares", "CompanyID");
            CreateIndex("dbo.Questionares", "ApplicationUserId");
            CreateIndex("dbo.Questionares", "CodeName");
            AddForeignKey("dbo.Questionares", "CompanyID", "dbo.Companies", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Questionares", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
