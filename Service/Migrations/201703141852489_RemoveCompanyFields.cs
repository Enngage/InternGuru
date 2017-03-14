namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCompanyFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Companies", "YearFounded");
            DropColumn("dbo.Companies", "PublicEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "PublicEmail", c => c.String());
            AddColumn("dbo.Companies", "YearFounded", c => c.Int(nullable: false));
        }
    }
}
