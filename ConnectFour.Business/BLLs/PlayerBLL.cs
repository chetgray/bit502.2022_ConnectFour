using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Business.BLLs
{
    public class PlayerBLL
    {
        public IPlayerModel AddPlayerToRoom(IPlayerModel playerModel, IRoomModel roomModel)
        {
            int playerNum = (roomModel.Players[0].Num == 1) ? 2 : 1;
            playerModel.Num = playerNum;
            PlayerRepository pRepo = new PlayerRepository();
            PlayerDTO playerDTO = ConvertToDto(playerModel);
            playerDTO.RoomId = roomModel.Id;
            playerDTO = pRepo.AddPlayerToRoom(playerDTO);
            return ConvertToModel(playerDTO);
        }
        private static PlayerDTO ConvertToDto(IPlayerModel playerModel)
        {
            PlayerDTO pDTO = new PlayerDTO();
            pDTO.Id = playerModel.Id;
            pDTO.Name = playerModel.Name;
            pDTO.Num = playerModel.Num;
            return pDTO;
        }

        private static IPlayerModel ConvertToModel(PlayerDTO dto)
        {
            PlayerModel pM = new PlayerModel();
            pM.Id = dto.Id;
            pM.Name = dto.Name;
            pM.Num = dto.Num;
            return pM;
        }
    }
}
