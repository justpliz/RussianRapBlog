using Microsoft.AspNetCore.Identity;

namespace Models
{
    /// <summary>
    ///     Пользователь
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Рейтинг пользователя
        /// </summary>
        public int Rating { get; set; }
    }
}