using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Tests.TestDoubles
{
    internal class PlayerBLLStub : IPlayerBLL
    {
        public IPlayerModel TestModel { get; set; }
        public IPlayerModel[] TestModels { get; set; }

        public IPlayerModel AddPlayerToRoom(IPlayerModel model, int roomId)
        {
            return TestModel;
        }

        public IPlayerModel[] GetPlayersInRoom(int roomId)
        {
            return TestModels;
        }
    }
}
