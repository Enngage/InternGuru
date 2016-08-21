namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Url = c.String(),
                        ExceptionMessage = c.String(),
                        InnerException = c.String(),
                        Stacktrace = c.String(),
                        ApplicationUserName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
