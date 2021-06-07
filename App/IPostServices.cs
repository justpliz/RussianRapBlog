
using System.Threading.Tasks;
using Dto;

namespace Services
{
    /// <summary>
    /// Интерфейс для работы с постами
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Получить пост из базы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PostOutDto> GetPostAsync(int id);

        /// <summary>
        /// Создать новый пост
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task CreatePostAsync(string text); //TODO возврат поста
    }
}
