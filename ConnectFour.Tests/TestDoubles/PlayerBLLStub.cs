using System.Collections.Generic;

using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Tests.TestDoubles
{
    internal class PlayerBLLStub : IPlayerBLL
    {
        public IPlayerModel TestModel { get; set; }
        public List<IPlayerModel> TestModels { get; set; }

        public IPlayerModel AddPlayerToRoom(IPlayerModel model, int roomId)
        {
            return TestModel;
        }

        public List<IPlayerModel> GetPlayersInRoom(int roomId)
        {
            return TestModels;
        }
    }
}
