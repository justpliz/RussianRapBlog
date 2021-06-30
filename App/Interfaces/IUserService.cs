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
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<string> RegisterAsync(RegisterDto dto);
    }
}