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
            Images = new Collection<ImageModel>();
            Voters = new Collection<Voter>();
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
        public ICollection<ImageModel> Images { get; set; }

        /// <summary>
        ///     Автор
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        ///     Рейтинг поста
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        ///     Оценившие пост
        /// </summary>
        public ICollection<Voter> Voters { get; set; }

        /// <Inheritdoc />
        public int Id { get; set; }
    }
}