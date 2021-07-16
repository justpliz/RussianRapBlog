using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Models;

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
        private readonly UserManager<User> _userManager;

        public PostController(IPostService postService, UserManager<User> userManager)
        {
            _userManager = userManager;
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
            var result = await _postService.GetPostAsync(id).ConfigureAwait(false);
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
            var user = await _userManager.GetUserAsync(User);
            await _postService.CreatePostAsync(text, images, user).ConfigureAwait(false);
        }

        [Authorize(Roles ="User")]
        [HttpPost]
        public async Task<long> UpVoteAsync(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _postService.UpVoteAsync(postId, user).ConfigureAwait(false);
        }

        [Authorize(Roles ="User")]
        [HttpPost]
        public async Task<long> DownVoteAsync(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            return await _postService.DownVoteAsync(postId, user).ConfigureAwait(false);
        }
    }
}