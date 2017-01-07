namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Activities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActivityType = c.String(maxLength: 50),
                        ApplicationUserId = c.String(maxLength: 128),
                        ActivityDateTime = c.DateTime(nullable: false),
                        RelevantCompanyID = c.Int(),
                        ObjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Companies", t => t.RelevantCompanyID)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.RelevantCompanyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "RelevantCompanyID", "dbo.Companies");
            DropForeignKey("dbo.Activities", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Activities", new[] { "RelevantCompanyID" });
            DropIndex("dbo.Activities", new[] { "ApplicationUserId" });
            DropTable("dbo.Activities");
        }
    }
}
