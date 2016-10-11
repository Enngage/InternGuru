namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrencyShowSignOnLeftColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currencies", "ShowSignOnLeft", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Currencies", "ShowSignOnLeft");
        }
    }
}
