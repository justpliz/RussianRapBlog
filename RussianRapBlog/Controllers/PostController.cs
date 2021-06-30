using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

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
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            return await _postService.GetPostAsync(id);
        }

        /// <summary>
        ///     Получить изображения поста
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Изображения</returns>
        [HttpGet("images/{id}")]
        public async Task<List<FileContentResult>> GetPostImagesAsync(int id)
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
        [HttpPost("{text}")] //TODO Валидация изображений
        public async Task CreatePostAsync(string text, [FromForm] IFormFileCollection images)
        {
            await using var imageStream = new MemoryStream();
            var splittedImages = new List<ImageModel>();
            foreach (var image in images)
            {
                await image.CopyToAsync(imageStream);
                splittedImages.Add(new ImageModel {Data = imageStream.GetBuffer()});
            }

            await _postService.CreatePostAsync(text, splittedImages);
        }
    }
}