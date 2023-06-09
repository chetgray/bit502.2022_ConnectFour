﻿using System;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class PlayerModel : IPlayerModel
    {
        public int? Id { get; set; }

        public ConsoleColor Color { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Num { get; set; }
        public string Symbol { get; set; }
    }
}
