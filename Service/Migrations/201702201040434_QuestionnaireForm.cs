namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionnaireSubmissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeName = c.String(nullable: false, maxLength: 50),
                        CreatedByApplicationUserId = c.String(nullable: false, maxLength: 128),
                        UpdatedByApplicationUserId = c.String(nullable: false, maxLength: 128),
                        QuestionnaireID = c.Int(nullable: false),
                        SubmissionXml = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireID)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedByApplicationUserId)
                .Index(t => t.CodeName)
                .Index(t => t.CreatedByApplicationUserId)
                .Index(t => t.UpdatedByApplicationUserId)
                .Index(t => t.QuestionnaireID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireSubmissions", "UpdatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionnaireSubmissions", "QuestionnaireID", "dbo.Questionnaires");
            DropForeignKey("dbo.QuestionnaireSubmissions", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.QuestionnaireSubmissions", new[] { "QuestionnaireID" });
            DropIndex("dbo.QuestionnaireSubmissions", new[] { "UpdatedByApplicationUserId" });
            DropIndex("dbo.QuestionnaireSubmissions", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.QuestionnaireSubmissions", new[] { "CodeName" });
            DropTable("dbo.QuestionnaireSubmissions");
        }
    }
}
