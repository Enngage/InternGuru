namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntAmountInternship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Internships", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Internships", "Amount", c => c.Double(nullable: false));
        }
    }
}
