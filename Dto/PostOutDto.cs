using System;

namespace Dto
{
    /// <summary>
    /// Dto поста
    /// </summary>
    public class PostOutDto
    {
        /// <summary>
        /// Текст поста
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания поста
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
