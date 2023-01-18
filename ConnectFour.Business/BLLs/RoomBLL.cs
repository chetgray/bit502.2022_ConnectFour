using System;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;

namespace ConnectFour.Business.BLLs
{
    public class RoomBLL
    {
        public IPlayerModel AddPlayerToRoom(IPlayerModel playerModel, IRoomModel roomModel)
        {
            int playerNum = (roomModel.Players[0].Num == 1) ? 2 : 1;
            playerModel.Num = playerNum;
            PlayerRepository pRepo = new PlayerRepository();
            PlayerBLL pBLL = new PlayerBLL();
            PlayerDTO playerDTO = pBLL.ConvertToDto(playerModel);
            playerDTO.RoomId = roomModel.Id;
            playerDTO = pRepo.AddPlayerToRoom(playerDTO);
            return pBLL.ConvertToModel(playerDTO);
        }
        public IRoomModel GetRoomOccupancy(int roomId)
        {
            RoomRepository roomRepo = new RoomRepository();
            IRoomModel room = ConvertToModel(roomRepo.GetRoomOccupancy(roomId));
            return room;
        }
        private RoomDTO ConvertToDto(IRoomModel model)
        {
            throw new NotImplementedException();
        }
        private IRoomModel ConvertToModel(RoomDTO dto)
        {
            RoomModel rM = new RoomModel();
            rM.Id = dto.Id;
            rM.CreationTime = dto.CreationTime;
            rM.CurrentTurnNum = dto.CurrentTurnNumber;
            rM.ResultCode = dto.ResultCode;
            PlayerBLL pBLL = new PlayerBLL();
            foreach (PlayerDTO pDTO in dto.Players)
            {
                rM.Players.Add(pBLL.ConvertToModel(pDTO));
            }
            rM.Vacancy = (rM.Players.Count < 2) ? true : false;            
            return rM;
        }
    }
}
