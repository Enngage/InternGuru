namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationUserIdToCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Companies", "ApplicationUserId");
            AddForeignKey("dbo.Companies", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Companies", new[] { "ApplicationUserId" });
            DropColumn("dbo.Companies", "ApplicationUserId");
        }
    }
}
