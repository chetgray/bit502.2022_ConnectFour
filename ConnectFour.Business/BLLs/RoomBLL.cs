using System;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;

namespace ConnectFour.Business.BLLs
{
    public class RoomBLL
    {
        public IRoomModel GetRoomOccupancy(int roomId)
        {
            RoomRepository roomRepo = new RoomRepository();
            IRoomModel room = ConvertRoomDTOToModel(roomRepo.GetRoomOccupancy(roomId));
            return room;
        }

        private static PlayerModel ConvertPlayerDTOToModel(PlayerDTO dto)
        {
            PlayerModel pM = new PlayerModel();
            pM.Id = dto.Id;
            pM.Name = dto.Name;
            pM.Num = dto.Num;
            return pM;
        }

        private static IPlayerModel ConvertToModel(PlayerDTO dto)
        {
            throw new NotImplementedException();
        }
        private static RoomModel ConvertRoomDTOToModel(RoomDTO roomDTO)
        {
            RoomModel rM = new RoomModel();
            rM.Id = roomDTO.Id;
            rM.CreationTime = roomDTO.CreationTime;
            rM.CurrentTurnNum = roomDTO.CurrentTurnNumber;
            rM.ResultCode = roomDTO.ResultCode;
            foreach (PlayerDTO pDTO in roomDTO.Players)
            {
                rM.Players.Add(ConvertPlayerDTOToModel(pDTO));
            }
            rM.Vacancy = (rM.Players.Count < 2) ? true : false;            
            return rM;
        }
    }
}
