using System.Collections.Generic;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    /// <summary>
    ///     Интерфейс для работы с постами
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        ///     Получить пост из базы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PostOutDto> GetPostAsync(int id);

        /// <summary>
        ///     Создать новый пост
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task CreatePostAsync(string text, IFormFileCollection images); //TODO возврат поста

        /// <summary>
        ///     Получить изображения поста
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Список изображений</returns>
        public Task<List<byte[]>> GetPostImagesAsync(int id);
    }
}