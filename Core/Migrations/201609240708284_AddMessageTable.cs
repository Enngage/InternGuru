namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        MessageCreated = c.DateTime(nullable: false),
                        MessageText = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Messages", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ApplicationUserId" });
            DropIndex("dbo.Messages", new[] { "CompanyID" });
            DropTable("dbo.Messages");
        }
    }
}
