using Models.Constants;
using Models.Interfaces;

namespace Models
{
    public class Voter:IEntity 
    {
        public int Id { get; set; }
        public User User{ get; set; }
        public Vote Vote{ get; set; } //TODO написать в конфиге, что юзер не может быть нулл
    }
}
