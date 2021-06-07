using System;
using Models.Interfaces;

namespace Models
{
    /// <summary>
    /// Пост в блоге
    /// </summary>
    public class Post:IEntity
    {
        ///<Inheritdoc/>
        public int Id { get; set; }

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
