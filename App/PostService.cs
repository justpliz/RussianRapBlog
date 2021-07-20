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

        public PostService(RussianRapBlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        /// <inheritdoc />
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            var post = await _blogContext.Posts.Include(i => i.Images).Include(p => p.Author)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return null;

            return new PostOutDto
            {
                Text = post.Text,
                CreationDate = post.CreationDate.ToShortDateString(),
                Images = post.Images.Select(i => i.Data).ToList(),
                Rating = post.Rating,
                Author = post?.Author.UserName
            };
        }

        /// <inheritdoc />
        public async Task<PostOutDto>
            CreatePostAsync(string text, IFormFileCollection images, User user) //TODO возврат поста
        {
            var author = await _blogContext.Users.SingleOrDefaultAsync(p => p.UserName == user.UserName);
            var post = new Post
            {
                Text = text,
                CreationDate = DateTime.Now,
                Images = await SplitImages(images).ConfigureAwait(false),
                Author = author
            };
            await _blogContext.Posts.AddAsync(post).ConfigureAwait(false);

            await _blogContext.SaveChangesAsync().ConfigureAwait(false);

            return new PostOutDto //TODO написать мапперы для исключения повторов
            {
                Text = post.Text,
                CreationDate = post.CreationDate.ToShortDateString(),
                Images = post.Images.Select(i => i.Data).ToList(),
                Rating = post.Rating,
                Author = author.UserName
            };
        }

        /// <inheritdoc />
        public async Task<string> VoteAsync(int postId, User user, Vote vote)
        {
            var post = await _blogContext.Posts.Include(p => p.Voters.Where(u => u.User == user))
                .FirstOrDefaultAsync(i => i.Id == postId);
            if (post == null)
                throw new NotFoundException($"Пост с id {postId} не найден");

            var voter = post.Voters.FirstOrDefault();
            if (voter?.Vote == vote)
                return "Вы уже так проголосовали";

            if (voter == null)
            {
                if (vote == Vote.Up)
                    post.Rating++;
                else
                    post.Rating--;

                post.Voters.Add(new Voter {User = user, Vote = vote});
            }
            else
            {
                if (vote == Vote.Up)
                    post.Rating += 2;
                else
                    post.Rating -= 2;

                voter.Vote = vote;
            }

            await _blogContext.SaveChangesAsync().ConfigureAwait(false);
            return post.Rating.ToString();
        }

        /// <inheritdoc />
        public async Task<string> RemovePostAsync(int postId, User user)
        {
            var post = await _blogContext.Posts.Include(p => p.Author).SingleOrDefaultAsync(p => p.Id == postId)
                .ConfigureAwait(false);
            if (post == null)
                throw new NotFoundException($"Пост с Id {postId} не найден.");

            if (post.Author.Id != user.Id)
                return "Пост может удалить только автор";

            _blogContext.Posts.Remove(post);
            await _blogContext.SaveChangesAsync().ConfigureAwait(false);

            return $"Пост {postId} успешно удален";
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