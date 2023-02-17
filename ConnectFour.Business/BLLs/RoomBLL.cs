using System;
using System.Collections.Generic;

using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;


namespace ConnectFour.Business.BLLs
{
    public class RoomBLL : IRoomBLL
    {
        private IRoomRepository _repository;

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public RoomBLL()
        {
            _repository = new RoomRepository();
        }

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with the passed <paramref name="repository"/>
        /// as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use as the backend.</param>
        public RoomBLL(IRoomRepository repository)
        {
            _repository =
                repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<IResultModel> GetAllFinished()
        {
            List<IResultModel> resultModels = new List<IResultModel>();
            List<ResultDTO> resultDTOs = _repository.GetAllFinished();
            for (int i = 0; i < resultDTOs.Count; i++)
            {
                resultModels.Add(ConvertToResultModel(resultDTOs[i]));
            }
            return resultModels;
        }

        private ResultModel ConvertToResultModel(ResultDTO dto)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.RoomId = dto.RoomId;
            resultModel.CreationTime = dto.CreationTime;
            resultModel.Duration = GetGameDuration(dto.LastTurnTime - resultModel.CreationTime);
            string[] playerNames = new string[dto.Players.Count];
            foreach (KeyValuePair<int, string> player in dto.Players)
            {
                playerNames[player.Key - 1] = player.Value;
                if (player.Value.Length > 15)
                {
                    playerNames[player.Key - 1] = $"{player.Value.Substring(0, 15)}...";
                }
            }
            resultModel.Players = playerNames;
            resultModel.ResultCode = dto.ResultCode;
            resultModel.WinnerName = DetermineWinner(dto.ResultCode, dto.Players);
            resultModel.LastTurnNum = dto.LastTurnNum.ToString();
            return resultModel;
        }

        private static string GetGameDuration(TimeSpan duration)
        {
            int days = (int)duration.TotalDays;
            int hours = (int)duration.TotalHours;
            int minutes = (int)duration.TotalMinutes;
            if (days >= 1)
            {
                if (days > 1)
                {
                    return $"{days} Days";
                }
                else
                {
                    return $"{days} Day";
                }
            }
            else if (hours >= 1)
            {
                if (hours > 1)
                {
                    return $"{hours} Hours";
                }
                else
                {
                    return $"{hours} Hour";
                }
            }
            else
            {
                if (minutes > 1)
                {
                    return $"{minutes} Minutes";
                }
                else
                {
                    return $"{minutes} Minute";
                }
            }
        }

        private static string DetermineWinner(int? resultCode, Dictionary<int, string> players)
        {
            string winnerName = string.Empty;
            if (resultCode > 0 && resultCode < 3)
            {
                winnerName = $"{players[(int)resultCode]}";
                if (winnerName.Length > 15)
                {
                    winnerName = $"{winnerName.Substring(0, 15)}...";
                }
            }
            else if (resultCode == 0)
            {
                winnerName = "DRAW";
            }
            else
            {
                winnerName = "NULL";
            }
            return winnerName;
        }

        internal RoomDTO ConvertToDto(IRoomModel model)
        {
            throw new NotImplementedException();
        }

        internal IRoomModel ConvertToModel(RoomDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
