namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTypeFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsCompany", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsCandidate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsCandidate");
            DropColumn("dbo.AspNetUsers", "IsCompany");
        }
    }
}
