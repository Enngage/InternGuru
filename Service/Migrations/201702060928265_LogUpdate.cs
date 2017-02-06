namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Updated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Updated");
        }
    }
}
