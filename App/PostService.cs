﻿using System;
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
        private readonly RussianRapBlogContext _context;

        public PostService(RussianRapBlogContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<PostOutDto> GetPostAsync(int id)
        {
            var post = await _context.Posts.Include(i => i.Images).SingleOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return null;

            return new PostOutDto
            {
                Text = post.Text,
                CreationDate = post.CreationDate.ToShortDateString(),
                Images = post.Images.Select(i => i.Data).ToList()
            };
        }

        /// <inheritdoc />
        public async Task CreatePostAsync(string text, IFormFileCollection images) //TODO возврат поста
        {
            await _context.Posts.AddAsync(new Post
                {Text = text, CreationDate = DateTime.Now, Images = await SplitImages(images)});
            await _context.SaveChangesAsync();
        }

        private async Task<List<ImageModel>> SplitImages(IFormFileCollection images)
        {
            await using var imageStream = new MemoryStream();
            var splittedImages = new List<ImageModel>();
            foreach (var image in images)
            {
                await image.CopyToAsync(imageStream);
                try
                {
                    Image.FromStream(imageStream);
                    splittedImages.Add(new ImageModel {Data = imageStream.GetBuffer(), Name = image.FileName});
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