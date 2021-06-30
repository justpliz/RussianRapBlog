using System.ComponentModel.DataAnnotations;

namespace Dto
{
    /// <summary>
    ///     Dto регистрации нового пользователя
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        ///     Имя пользователя
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///     Пароль
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public string Description { get; set; }
    }
}