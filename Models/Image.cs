using Models.Interfaces;

namespace Models
{
    /// <summary>
    ///     Изображение
    /// </summary>
    public class Image : IEntity
    {
        /// <summary>
        ///     Имя изображения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Данные изображения
        /// </summary>
        public byte[] Data { get; set; }

        /// <inheritdoc />
        public int Id { get; set; }
    }
}