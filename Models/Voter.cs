using Models.Constants;
using Models.Interfaces;

namespace Models
{
    /// <summary>
    /// Запись об оценке поста
    /// </summary>
    public class Voter : IEntity 
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Автор оценки
        /// </summary>
        public User User{ get; set; }

        /// <summary>
        /// Тип оценки
        /// </summary>
        public Vote Vote{ get; set; } //TODO написать в конфиге, что юзер не может быть нулл
    }
}
