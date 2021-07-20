using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RussianRapBlog.Extensions
{
    /// <summary>
    ///     Расширение для подключения БД
    /// </summary>
    public static class DatabaseExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<RussianRapBlogContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(RussianRapBlogContext).Assembly.FullName)));
        }
    }
}