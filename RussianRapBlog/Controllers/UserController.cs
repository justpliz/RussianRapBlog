using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace RussianRapBlog.Controllers
{
    /// <summary>
    ///     Контроллер пользователей
    /// </summary>
    [Route("api/[controller]")]
    [RequireHttps]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Регистрация нового пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")] //TODO Валидация данных
        public async Task<ActionResult> RegisterAsync(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        ///     Получить токен авторизации
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestDto dto)
        {
            var result = await _userService.GetTokenAsync(dto).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        ///     Получить данные о пользователе
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user")]
        public async Task<UserOutDto> GetUserAsync(string userName)
        {
            return await _userService.GetUserAsync(userName).ConfigureAwait(false);
        }
    }
}