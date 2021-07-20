namespace Dto
{
    /// <summary>
    ///     Дто вывода пользователя
    /// </summary>
    public class UserOutDto
    {
        /// <summary>
        ///     Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Описание пользователя
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Рейтинг пользователя
        /// </summary>
        public int Rating { get; set; }
    }
}