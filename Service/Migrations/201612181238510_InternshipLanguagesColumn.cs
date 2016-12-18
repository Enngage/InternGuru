namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternshipLanguagesColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Internships", "Languages", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Internships", "Languages");
        }
    }
}
