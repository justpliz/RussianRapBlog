using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    /// <summary>
    ///     Контекст приложения
    /// </summary>
    public class RussianRapBlogContext : IdentityDbContext<User>
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public RussianRapBlogContext(DbContextOptions<RussianRapBlogContext> options) : base(options)
        {
            Database.Migrate();
        }

        /// <summary>
        ///     Посты
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        ///     Изображения
        /// </summary>
        public DbSet<ImageModel> Images { get; set; }
    }
}