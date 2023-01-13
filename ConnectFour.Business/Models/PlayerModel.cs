using ConnectFour.Business.Models.Interfaces;
using System;

namespace ConnectFour.Business.Models
{
    public class PlayerModel : IPlayerModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; }
        public ConsoleColor Color { get; set; }
        public int Num { get; set; }
    }
}
