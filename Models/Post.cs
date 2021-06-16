using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Models.Interfaces;

namespace Models
{
    /// <summary>
    ///     Пост в блоге
    /// </summary>
    public class Post : IEntity
    {
        public Post()
        {
            Images = new Collection<Image>();
        }

        /// <summary>
        ///     Текст поста
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Дата создания поста
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        ///     Изображения
        /// </summary>
        public ICollection<Image> Images { get; set; }

        /// <Inheritdoc />
        public int Id { get; set; }
    }
}