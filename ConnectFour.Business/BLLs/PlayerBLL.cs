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
        public IPlayerModel AddPlayerToRoom(IPlayerModel model, int roomId)
        {
            PlayerRepository pRepo = new PlayerRepository();
            PlayerDTO dto = ConvertToDto(model);
            dto.RoomId = roomId;
            dto = pRepo.AddPlayerToRoom(dto);
            return ConvertToModel(dto);
        }
        internal PlayerDTO ConvertToDto(IPlayerModel playerModel)
        {
            PlayerDTO pDTO = new PlayerDTO();
            pDTO.Id = playerModel.Id;
            pDTO.Name = playerModel.Name;
            pDTO.Num = playerModel.Num;
            return pDTO;
        }

        internal IPlayerModel ConvertToModel(PlayerDTO dto)
        {
            PlayerModel pM = new PlayerModel();
            pM.Id = dto.Id;
            pM.Name = dto.Name;
            pM.Num = dto.Num;
            pM.Symbol = dto.Name.Substring(0, 1).ToUpper();
            pM.Color = (pM.Num == 1) ? ConsoleColor.Red : ConsoleColor.Yellow;
            return pM;
        }
    }
}
