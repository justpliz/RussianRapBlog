using System.Collections.Generic;
using System.Linq;
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
        ///     Получить изображения поста
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Изображения</returns>
        [HttpGet("images/{id}")]
        public async Task<List<FileContentResult>> GetPostImagesAsync(int id) //TODO Разобраться. Есть ощущение, что так делать не стоит
        {
            var images = await _postService.GetPostImagesAsync(id);
            var result = images.Select(p => new FileContentResult(p, "image/jpeg")).ToList();
            return result;
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