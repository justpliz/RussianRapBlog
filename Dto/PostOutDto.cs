using System.Collections.Generic;

namespace Dto
{
    /// <summary>
    ///     Dto поста
    /// </summary>
    public class PostOutDto
    {
        /// <summary>
        ///     Текст поста
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Дата создания поста
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        ///     Изображения
        /// </summary>
        public List<byte[]> Images { get; set; }

        /// <summary>
        /// Рейтинг поста
        /// </summary>
        public int Rating { get; set; }
    }
}