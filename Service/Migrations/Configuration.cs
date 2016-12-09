namespace Service.Migrations
{
    using Core.Helpers.Internship;
    using Context;
    using Entity;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }
}