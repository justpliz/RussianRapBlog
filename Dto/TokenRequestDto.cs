using System.ComponentModel.DataAnnotations;

namespace Dto
{
    /// <summary>
    ///     Dto запроса токена авторизации
    /// </summary>
    public class TokenRequestDto
    {
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
    }
}