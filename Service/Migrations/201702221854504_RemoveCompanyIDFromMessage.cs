namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCompanyIDFromMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "RecipientCompanyID", "dbo.Companies");
            DropIndex("dbo.Messages", new[] { "RecipientCompanyID" });
            RenameColumn(table: "dbo.Messages", name: "RecipientCompanyID", newName: "Company_ID");
            AlterColumn("dbo.Messages", "Company_ID", c => c.Int());
            CreateIndex("dbo.Messages", "Company_ID");
            AddForeignKey("dbo.Messages", "Company_ID", "dbo.Companies", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Company_ID", "dbo.Companies");
            DropIndex("dbo.Messages", new[] { "Company_ID" });
            AlterColumn("dbo.Messages", "Company_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Messages", name: "Company_ID", newName: "RecipientCompanyID");
            CreateIndex("dbo.Messages", "RecipientCompanyID");
            AddForeignKey("dbo.Messages", "RecipientCompanyID", "dbo.Companies", "ID", cascadeDelete: true);
        }
    }
}
