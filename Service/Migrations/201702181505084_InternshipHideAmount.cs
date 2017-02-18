namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipHideAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "HideAmount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "HideAmount");
        }
    }
}
