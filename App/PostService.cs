using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Constants;
using Models.Exceptions;
using Services.Interfaces;

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
        private readonly RussianRapBlogContext _blogContext;
        private readonly IdentityDbContext _identityContext;

        public PostService(RussianRapBlogContext blogContext, IdentityDbContext identityContext)
        {
            _blogContext = blogContext;
            _identityContext = identityContext;
        }

        /// <inheritdoc />
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            var post = await _blogContext.Posts.Include(i => i.Images).SingleOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return null;

            return new PostOutDto
            {
                Text = post.Text,
                CreationDate = post.CreationDate.ToShortDateString(),
                Images = post.Images.Select(i => i.Data).ToList(),
                Rating = post.Rating
            };
        }

        /// <inheritdoc />
        public async Task CreatePostAsync(string text, IFormFileCollection images, User user) //TODO возврат поста
        {
            await _blogContext.Posts.AddAsync(new Post
            {
                Text = text,
                CreationDate = DateTime.Now,
                Images = await SplitImages(images).ConfigureAwait(false),
                Author = identityUser
            });

            await _blogContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<long> VoteAsync(int postId, User user,Vote vote)
        {
            var post = await _blogContext.Posts.Include(p => p.Voters.Where(u => u.User == user)).FirstOrDefaultAsync(i=>i.Id == postId);
            if (post == null)
                throw new NotFoundException($"Пост с id {postId} не найден");

            var voter = post.Voters.FirstOrDefault();
            if (voter?.Vote == vote)
                return post.Rating;

            if (voter == null)
            {
                if (vote == Vote.Up)
                    post.Rating++;
                else
                    post.Rating--;

                post.Voters.Add(new Voter() { User = user, Vote = vote });
            }
            else
            {
                if (vote == Vote.Up)
                    post.Rating -= 2;
                else
                    post.Rating += 2;

                post.Voters.Remove(voter);
                post.Voters.Add(new Voter() { User = user, Vote = Vote.Up });
            }

          await _blogContext.SaveChangesAsync().ConfigureAwait(false);
          return post.Rating;
        }

        private static async Task<List<ImageModel>> SplitImages(IFormFileCollection images)
        {
            await using var imageStream = new MemoryStream();
            var splittedImages = new List<ImageModel>();
            foreach (var image in images)
            {
                await image.CopyToAsync(imageStream).ConfigureAwait(false);
                try
                {
                    Image.FromStream(imageStream);
                    splittedImages.Add(new ImageModel
                    {
                        Data = imageStream.GetBuffer(), Name = image.FileName.Split("\\").LastOrDefault()
                    }); //TODO Подумать, выглядит как всратый костыль
                }
                catch (ArgumentException)
                {
                    throw new BusinessException($"Некорректное изображение {image.FileName}");
                }
            }

            return splittedImages;
        }
    }
}