namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 100),
                        To = c.String(maxLength: 200),
                        From = c.String(maxLength: 200),
                        HtmlBody = c.String(),
                        IsSent = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Sent = c.DateTime(),
                        Result = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Emails");
        }
    }
}
