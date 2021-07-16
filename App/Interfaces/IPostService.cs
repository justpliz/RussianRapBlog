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
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreatePostAsync(string text, IFormFileCollection images, string userId); //TODO возврат поста
    }
}