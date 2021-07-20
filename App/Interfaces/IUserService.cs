using System.Threading.Tasks;
using Dto;

namespace Services.Interfaces
{
    /// <summary>
    ///     Сервис для работы с пользователями
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     Зарегистрировать нового пользователя
        /// </summary>
        /// <param name="dto">Dto пользователя</param>
        /// <returns>Ответ</returns>
        public Task<string> RegisterAsync(RegisterDto dto);

        /// <summary>
        ///     Получить токен авторизации
        /// </summary>
        /// <param name="dto">Dto авторизации</param>
        /// <returns>Ответ на авторизацию</returns>
        Task<AuthenticationResponseDto> GetTokenAsync(TokenRequestDto dto);

        /// <summary>
        ///     Получить пользователя по имени
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<UserOutDto> GetUserAsync(string userName);
    }
}