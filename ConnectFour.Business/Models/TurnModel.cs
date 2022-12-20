﻿using System;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    internal class TurnModel : ITurnModel
    {
        public int? Id { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public int RowNum { get; set; }
        public int ColNum { get; set; }
    }
}
