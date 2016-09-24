namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecipientCompanyID : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "CompanyID", newName: "RecipientCompanyID");
            RenameIndex(table: "dbo.Messages", name: "IX_CompanyID", newName: "IX_RecipientCompanyID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Messages", name: "IX_RecipientCompanyID", newName: "IX_CompanyID");
            RenameColumn(table: "dbo.Messages", name: "RecipientCompanyID", newName: "CompanyID");
        }
    }
}
