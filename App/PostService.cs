using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Dto;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    /// <summary>
    ///     Сервис для работы с постами
    /// </summary>
    public class PostService : IPostService
    {
        /// <summary>
        ///     Контекст
        /// </summary>
        private readonly RussianRapBlogContext _context;

        public PostService(RussianRapBlogContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
            return new PostOutDto
            {
                Text = post.Text,
                CreationDate = post.CreationDate.ToShortDateString()
            };
        }

        /// <inheritdoc />
        public async Task CreatePostAsync(string text, List<Image> images) //TODO возврат поста
        {
            await _context.Posts.AddAsync(new Post {Text = text, CreationDate = DateTime.Now, Images = images});
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<byte[]>> GetPostImagesAsync(int id)
        {
            var post = await _context.Posts.Include(o => o.Images).SingleOrDefaultAsync(p => p.Id == id);
            return post.Images.Select(p => p.Data).ToList();
        }
    }
}