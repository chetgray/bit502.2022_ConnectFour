using System;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IPlayerModel
    {
        int? Id { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
        ConsoleColor Color { get; set; }
    }
}