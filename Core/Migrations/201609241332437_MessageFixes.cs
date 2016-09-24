namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageFixes : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "ApplicationUserId", newName: "RecipientApplicationUserId");
            RenameIndex(table: "dbo.Messages", name: "IX_ApplicationUserId", newName: "IX_RecipientApplicationUserId");
            AddColumn("dbo.Messages", "SenderApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "Subject", c => c.String(maxLength: 200));
            CreateIndex("dbo.Messages", "SenderApplicationUserId");
            AddForeignKey("dbo.Messages", "SenderApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenderApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "SenderApplicationUserId" });
            DropColumn("dbo.Messages", "Subject");
            DropColumn("dbo.Messages", "SenderApplicationUserId");
            RenameIndex(table: "dbo.Messages", name: "IX_RecipientApplicationUserId", newName: "IX_ApplicationUserId");
            RenameColumn(table: "dbo.Messages", name: "RecipientApplicationUserId", newName: "ApplicationUserId");
        }
    }
}
