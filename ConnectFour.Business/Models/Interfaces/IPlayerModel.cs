using System;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IPlayerModel
    {
        int? Id { get; set; }

        ConsoleColor Color { get; set; }
        string Name { get; set; }
        int Num { get; set; }
        string Symbol { get; set; }
    }
}
