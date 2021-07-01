using System.Collections.Generic;

namespace Dto
{
    /// <summary>
    ///     Ответ на попытку аутентификации
    /// </summary>
    public class AuthenticationResponseDto
    {
        /// <summary>
        ///     Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Успешна ли аутентификация
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        ///     Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Email пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Роли пользователя
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        ///     Токен
        /// </summary>
        public string Token { get; set; }
    }
}