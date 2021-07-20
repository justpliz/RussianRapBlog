using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Http;
using Models;
using Models.Constants;

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
        Task<PostOutDto> CreatePostAsync(string text, IFormFileCollection images, User user); //TODO возврат поста

        /// <summary>
        ///     Поставить оценку посту
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> VoteAsync(int postId, User user, Vote vote);

        /// <summary>
        ///     Удалить пост
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<string> RemovePostAsync(int postId, User user);
    }
}