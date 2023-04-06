using System;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface ITurnModel
    {
        int ColNum { get; set; }
        int? Id { get; set; }
        int Num { get; set; }
        int RowNum { get; set; }
        DateTime Time { get; set; }
    }
}
