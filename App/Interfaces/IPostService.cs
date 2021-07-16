using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Http;

using Models;

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
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreatePostAsync(string text, IFormFileCollection images, User user); //TODO возврат поста

        /// <summary>
        /// Лайкнуть пост
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<long> UpVoteAsync(int postId, User user);

        /// <summary>
        /// Дизлайкнуть пост
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<long> DownVoteAsync(int postId, User user);
    }
}