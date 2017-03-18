namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCitiesToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SubscribedCities", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SubscribedCities");
        }
    }
}
