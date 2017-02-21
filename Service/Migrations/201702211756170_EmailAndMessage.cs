namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailAndMessage : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Messages", new[] { "SenderApplicationUserId" });
            DropIndex("dbo.Messages", new[] { "RecipientApplicationUserId" });
            AddColumn("dbo.Messages", "QuestionnaireSubmissionID", c => c.Int());
            AlterColumn("dbo.Messages", "SenderApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Messages", "RecipientApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Messages", "MessageText", c => c.String(nullable: false));
            CreateIndex("dbo.Messages", "SenderApplicationUserId");
            CreateIndex("dbo.Messages", "RecipientApplicationUserId");
            CreateIndex("dbo.Messages", "QuestionnaireSubmissionID");
            AddForeignKey("dbo.Messages", "QuestionnaireSubmissionID", "dbo.QuestionnaireSubmissions", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "QuestionnaireSubmissionID", "dbo.QuestionnaireSubmissions");
            DropIndex("dbo.Messages", new[] { "QuestionnaireSubmissionID" });
            DropIndex("dbo.Messages", new[] { "RecipientApplicationUserId" });
            DropIndex("dbo.Messages", new[] { "SenderApplicationUserId" });
            AlterColumn("dbo.Messages", "MessageText", c => c.String());
            AlterColumn("dbo.Messages", "RecipientApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Messages", "SenderApplicationUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Messages", "QuestionnaireSubmissionID");
            CreateIndex("dbo.Messages", "RecipientApplicationUserId");
            CreateIndex("dbo.Messages", "SenderApplicationUserId");
        }
    }
}
