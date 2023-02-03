using System;
using System.Collections.Generic;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;

namespace ConnectFour.Business.BLLs
{
    public class RoomBLL
    {
        public IRoomModel AddPlayerToRoom(string playerName, IRoomModel roomModel)
        {
            int playerNum = (roomModel.Players[0].Num == 1) ? 2 : 1;
            IPlayerModel playerModel = new PlayerModel
            {
                Name = playerName,
                Num = playerNum
            };
            PlayerBLL pBLL = new PlayerBLL();
            playerModel = pBLL.AddPlayerToRoom(playerModel, (int)roomModel.Id);
            roomModel.Players.Add(playerModel);
            return roomModel;
        }
        public IRoomModel GetRoomOccupancy(int roomId)
        {
            RoomRepository roomRepo = new RoomRepository();
            IRoomModel room = ConvertToModel(roomRepo.GetRoomOccupancy(roomId));
            if(room != null)
            {
                PlayerBLL pBLL = new PlayerBLL();
                List<IPlayerModel> players = pBLL.GetPlayersInRoom(roomId);
                if(players.Count < 2)
                {
                    foreach (IPlayerModel player in players)
                    {
                        room.Players.Add(player);
                    }
                    room.Vacancy = true;
                }
            }
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
            return rM;
        }
    }
}
