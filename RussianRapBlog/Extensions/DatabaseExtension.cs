using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace RussianRapBlog.Extensions
{
    /// <summary>
    ///     Расширение для подключения БД
    /// </summary>
    public static class DatabaseExtension
    {
        public static void AddDatabase(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>();
            services.AddDbContext<RussianRapBlogContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(RussianRapBlogContext).Assembly.FullName)));
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));
        }
    }
}