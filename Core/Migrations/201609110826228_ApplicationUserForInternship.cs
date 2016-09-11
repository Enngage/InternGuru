namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserForInternship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Internships", "ApplicationUserId");
            AddForeignKey("dbo.Internships", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Internships", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Internships", new[] { "ApplicationUserId" });
            DropColumn("dbo.Internships", "ApplicationUserId");
        }
    }
}
