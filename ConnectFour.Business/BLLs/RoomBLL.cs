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

        private static IPlayerModel ConvertPlayerDTOToModel(PlayerDTO dto)
        {
            PlayerModel pM = new PlayerModel();
            pM.Id = dto.Id;
            pM.Name = dto.Name;
            return pM;
        }
        private static ITurnModel ConvertTurnDTOToModel(TurnDTO dto)
        {
            TurnModel turnModel = new TurnModel();
            turnModel.Id = dto.Id;
            turnModel.ColNum = dto.ColNum;
            turnModel.RowNum = dto.RowNum;
            turnModel.Num = dto.Num;
            turnModel.Time = dto.Time;
            return turnModel;
        }
        private static ResultModel ConvertToResultModel(ResultDTO dto)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.CreationTime = dto.CreationTime;
            List<IPlayerModel> players = new List<IPlayerModel>();
            for (int i = 0; i < dto.Players.Count; i++)
            {
                players.Add(ConvertPlayerDTOToModel(dto.Players[i]));
            }
            resultModel.Players = players;
            resultModel.ResultCode = dto.ResultCode;
            resultModel.RoomId = dto.RoomId;
            resultModel.LastTurn = ConvertTurnDTOToModel(dto.LastTurn);
            return resultModel;
        }
    }
}
