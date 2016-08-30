namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyAddressFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Address", c => c.Int(nullable: false));
        }
    }
}
