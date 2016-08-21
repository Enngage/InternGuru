
namespace Core.Context
{
    public class Initializer : System.Data.Entity.CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {           
        }
    }
}