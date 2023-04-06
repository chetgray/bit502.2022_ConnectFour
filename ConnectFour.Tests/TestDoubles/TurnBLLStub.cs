using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Tests.TestDoubles
{
    internal class TurnBLLStub : ITurnBLL
    {
        public ITurnModel TestModel { get; set; }

        ITurnModel ITurnBLL.GetLatestTurnInRoom(int roomId)
        {
            return TestModel;
        }

        public void AddTurnToRoom(ITurnModel turn, int roomId) { }
    }
}
