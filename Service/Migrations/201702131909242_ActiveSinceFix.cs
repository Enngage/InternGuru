namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveSinceFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Internships", "ActiveSince", c => c.DateTime());
            AlterColumn("dbo.Theses", "ActiveSince", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Theses", "ActiveSince", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Internships", "ActiveSince", c => c.DateTime(nullable: false));
        }
    }
}
