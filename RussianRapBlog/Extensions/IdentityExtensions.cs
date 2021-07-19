using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace RussianRapBlog.Extensions
{
    /// <summary>
    ///     Расширение для подключения Microsoft Identity
    /// </summary>
    public static class IdentityExtensions
    {
        public static void AddIdentityConfigured(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<RussianRapBlogContext>();
        }
    }
}