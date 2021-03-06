using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Constants;
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
        public async Task<IActionResult> CreatePostAsync(string text, [FromForm] IFormFileCollection images)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ConfigureAwait(false);
            var result = await _postService.CreatePostAsync(text, images, user).ConfigureAwait(false);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        ///     Лайкнуть пост
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpPut("upvote")]
        public async Task<IActionResult> UpVoteAsync(int postId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ConfigureAwait(false);
            var result = await _postService.VoteAsync(postId, user, Vote.Up).ConfigureAwait(false);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        ///     Дизлайкнуть пост
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpPut("downvote")]
        public async Task<IActionResult> DownVoteAsync(int postId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ConfigureAwait(false);
            var result = await _postService.VoteAsync(postId, user, Vote.Down).ConfigureAwait(false);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemovePostAsync(int postId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ConfigureAwait(false);
            var result = await _postService.RemovePostAsync(postId, user).ConfigureAwait(false);
            return result == null ? NotFound() : Ok(result);
        }
    }
}