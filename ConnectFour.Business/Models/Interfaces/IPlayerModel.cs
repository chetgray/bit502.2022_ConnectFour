using System;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IPlayerModel
    {
        ConsoleColor Color { get; set; }
        int? Id { get; set; }
        string Name { get; set; }
        int Num { get; set; }
        string Symbol { get; set; }
    }
}
