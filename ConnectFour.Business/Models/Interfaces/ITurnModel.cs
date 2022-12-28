using System;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface ITurnModel
    {
        int? Id { get; set; }
        DateTime Time { get; set; }
        int RowNum { get; set; }
        int ColNum { get; set; }
        int Num { get; set; }
    }
}