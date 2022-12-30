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
        public static List<ResultModel> GetAllFinished()
        {
            List<ResultModel> resultModels = new List<ResultModel>();
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
            resultModel.CurrentTurnNum = dto.CurrentTurnNum;
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
            List<ITurnModel> turns = new List<ITurnModel>();
            for (int i = 0; i < dto.Turns.Count; i++)
            {
                ITurnModel turnModel = new TurnModel();
                turnModel.Id = dto.Turns[i].Id;
                turnModel.ColNum = dto.Turns[i].ColNum;
                turnModel.RowNum = dto.Turns[i].RowNum;
                turnModel.Num = dto.Turns[i].Num;
                turnModel.Time = dto.Turns[i].Time;
                turns.Add(turnModel);
            }
            resultModel.Turns = turns;
            return resultModel;
        }
    }
}
