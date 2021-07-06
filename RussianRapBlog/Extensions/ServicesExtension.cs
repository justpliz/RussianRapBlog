using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;

namespace RussianRapBlog.Extensions
{
    /// <summary>
    ///     Расширение для подключения сервисов
    /// </summary>
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}