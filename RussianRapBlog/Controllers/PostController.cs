using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace RussianRapBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet("{id}")]
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            return await _postService.GetPostAsync(id);
        }

        [HttpPost("{text}")]
        public async Task CreatePostAsync(string text)
        {
            await _postService.CreatePostAsync(text);
        }
    }
}
