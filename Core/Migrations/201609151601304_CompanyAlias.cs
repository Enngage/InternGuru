namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyAlias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyAlias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "CompanyAlias");
        }
    }
}
