using System;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class PlayerModel : IPlayerModel
    {
        public ConsoleColor Color { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Num { get; set; }
        public string Symbol { get; set; }
    }
}
