using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Tests.TestDoubles
{
    internal class TurnBLLStub : ITurnBLL
    {
        public ITurnModel TestModel { get; set; }

        public void AddTurnToRoom(ITurnModel turn, int roomId) { }

        ITurnModel ITurnBLL.GetLastTurnInRoom(int roomId)
        {
            return TestModel;
        }
    }
}
