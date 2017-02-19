namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireInThesisAndInternship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "QuestionnaireID", c => c.Int());
            AddColumn("dbo.Theses", "QuestionnaireID", c => c.Int());
            CreateIndex("dbo.Internships", "QuestionnaireID");
            CreateIndex("dbo.Theses", "QuestionnaireID");
            AddForeignKey("dbo.Theses", "QuestionnaireID", "dbo.Questionnaires", "ID");
            AddForeignKey("dbo.Internships", "QuestionnaireID", "dbo.Questionnaires", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Internships", "QuestionnaireID", "dbo.Questionnaires");
            DropForeignKey("dbo.Theses", "QuestionnaireID", "dbo.Questionnaires");
            DropIndex("dbo.Theses", new[] { "QuestionnaireID" });
            DropIndex("dbo.Internships", new[] { "QuestionnaireID" });
            DropColumn("dbo.Theses", "QuestionnaireID");
            DropColumn("dbo.Internships", "QuestionnaireID");
        }
    }
}
