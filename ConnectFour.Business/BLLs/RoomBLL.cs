using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;

namespace ConnectFour.Business.BLLs
{
    public class RoomBLL
    {
        public static List<IResultModel> GetAllFinished()
        {
            List<IResultModel> resultModels = new List<IResultModel>();
            RoomRepository roomRepository = new RoomRepository();
            List<ResultDTO> resultDTOs = roomRepository.GetAllFinished();
            for (int i = 0; i < resultDTOs.Count; i++)
            {
                resultModels.Add(ConvertToResultModel(resultDTOs[i]));
            }
            return resultModels;
        }
        private static PlayerDTO ConvertToDto(IPlayerModel playerModel)
        {
            throw new NotImplementedException();
        }

        private static IPlayerModel ConvertToModel(PlayerDTO dto)
        {
            throw new NotImplementedException();
        }
        private static ResultModel ConvertToResultModel(ResultDTO dto)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.CreationTime = dto.CreationTime;
            List<IPlayerModel> players = new List<IPlayerModel>();
            for (int i = 0; i < dto.Players.Count; i++)
            {
                IPlayerModel playerModel = new PlayerModel();
                playerModel.Id = dto.Players[i].Id;
                playerModel.Name = dto.Players[i].Name;
                players.Add(playerModel);
            }
            resultModel.Players = players;
            resultModel.ResultCode = dto.ResultCode;
            resultModel.RoomId = dto.RoomId;
            TurnModel turnModel = new TurnModel();
            turnModel.Id = dto.LastTurn.Id;
            turnModel.ColNum = dto.LastTurn.ColNum;
            turnModel.RowNum = dto.LastTurn.RowNum;
            turnModel.Num = dto.LastTurn.Num;
            turnModel.Time = dto.LastTurn.Time;
            resultModel.LastTurn = turnModel;
            return resultModel;
        }
    }
}
