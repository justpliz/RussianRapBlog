using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace RussianRapBlog.Controllers
{
    /// <summary>
    ///     Контроллер постов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        /// <summary>
        ///     Сервис постов
        /// </summary>
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        ///     Получить пост из бд
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Пост</returns>
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPostAsync(int id)
        {
            var result = await _postService.GetPostAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        ///     Создать пост
        /// </summary>
        /// <param name="text">Текст поста</param>
        /// <param name="images">Изображения</param>
        [Authorize(Roles = "User")]
        [HttpPost("{text}")]
        public async Task CreatePostAsync(string text, [FromForm] IFormFileCollection images)
        {
            await _postService.CreatePostAsync(text, images);
        }
    }
}