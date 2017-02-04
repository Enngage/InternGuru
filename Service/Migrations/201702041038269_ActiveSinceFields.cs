namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveSinceFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "ActiveSince", c => c.DateTime(nullable: false));
            AddColumn("dbo.Theses", "ActiveSince", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Theses", "ActiveSince");
            DropColumn("dbo.Internships", "ActiveSince");
        }
    }
}
