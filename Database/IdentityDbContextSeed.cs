using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Constants;

namespace Database
{
    /// <summary>
    ///     Автозаполнение дефолтных пользователей
    /// </summary>
    public class IdentityDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString())).ConfigureAwait(false);
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString())).ConfigureAwait(false);
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString())).ConfigureAwait(false);
        }
    }
}