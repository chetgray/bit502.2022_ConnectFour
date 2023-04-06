using System;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class TurnModel : ITurnModel
    {
        public int? Id { get; set; }

        public int ColNum { get; set; }
        public int Num { get; set; }
        public int RowNum { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
