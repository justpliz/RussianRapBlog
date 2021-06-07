using System;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Dto;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    /// <summary>
    /// Сервис для работы с постами
    /// </summary>
    public class PostService : IPostService
    {
        private readonly RussianRapBlogContext _context;

        public PostService(RussianRapBlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить пост из базы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
            return new PostOutDto()
            {
                Text = post.Text,
                CreationDate = post.CreationDate,
            };
        }

        /// <summary>
        /// Создать новый пост
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task CreatePostAsync(string text)//TODO возврат поста
        {
            await _context.Posts.AddAsync(new Post() {Text = text, CreationDate = DateTime.Now});
            await _context.SaveChangesAsync();
        }
    }
}
