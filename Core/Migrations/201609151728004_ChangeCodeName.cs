namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCodeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CodeName", c => c.String());
            DropColumn("dbo.Companies", "CompanyAlias");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "CompanyAlias", c => c.String());
            DropColumn("dbo.Companies", "CodeName");
        }
    }
}
