using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    /// <summary>
    ///     Контекст приложения
    /// </summary>
    public class RussianRapBlogContext : DbContext
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public RussianRapBlogContext(DbContextOptions<RussianRapBlogContext> options)
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
        public DbSet<Image> Images { get; set; }

        /// <summary>
        ///     Конфигурация
        /// </summary>
        /// <param name="optionsBuilder">Конструктор опций</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BlogDb;Trusted_Connection=True");
        }
    }
}