
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Context
{
    public class Initializer : System.Data.Entity.CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            RunSeed(context);
        }

        private void RunSeed(AppContext context)
        {
            // create admin role
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(context));
            var adminRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            adminRole.Name = "Admin";
            roleManager.Create(adminRole);

            // create admin user
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var newUser = new ApplicationUser()
            {
                UserName = "Enn",
                Email = "admin@email.com",

            };

            userManager.Create(newUser, "101154");

            // get this user
            var user = userManager.FindByName("Enn");

            // assign admin role to admin user
            userManager.AddToRole(user.Id, adminRole.Name);
        }
    }
}