namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmountType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "AmountType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "AmountType");
        }
    }
}
