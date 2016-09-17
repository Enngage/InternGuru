namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyNameIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Companies", "CompanyName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", new[] { "CompanyName" });
        }
    }
}
