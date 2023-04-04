using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        protected readonly IRoomRepository _repository;
        protected readonly IPlayerBLL _playerBLL;
        protected readonly ITurnBLL _turnBLL;

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public RoomBLL()
        {
            _repository = new RoomRepository();
            _playerBLL = new PlayerBLL();
            _turnBLL = new TurnBLL();
        }

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with the passed <paramref name="repository"/>,
        /// <paramref name="playerBLL"/>, and <paramref name="turnBLL"/> as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use in the backend.</param>
        /// <param name="playerBLL">The <see cref="IPlayerBLL"/> to use in the backend.</param>
        /// <param name="turnBLL">The <see cref="ITurnBLL"/> to use in the backend.</param>
        public RoomBLL(IRoomRepository repository, IPlayerBLL playerBLL, ITurnBLL turnBLL)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _playerBLL = playerBLL ?? throw new ArgumentNullException(nameof(playerBLL));
            _turnBLL = turnBLL ?? throw new ArgumentNullException(nameof(turnBLL));
        }

        public IRoomModel TryAddTurnToRoom(int colNum, IRoomModel room)
        {
            if (room.CurrentPlayerNum != room.LocalPlayerNum)
            {
                room.Message =
                    $"It's Not Your Turn! Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
                return room;
            }

            int rowNum;
            try
            {
                rowNum = room.GetNextRowInCol(colNum);
            }
            catch (ArgumentException e)
            {
                room.Message = e.Message;
                return room;
            }

            TurnModel turn = new TurnModel
            {
                ColNum = colNum,
                RowNum = rowNum,
                Num = room.CurrentTurnNum
            };

            room = AddTurnToRoom(turn, room);
            room.Message = 
                $"Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
            return room;
        }

        protected IRoomModel AddTurnToRoom(ITurnModel turn, IRoomModel room)
        {
            room.Turns.Add(turn);
            room.CurrentTurnNum++;
            room.ResultCode = DetermineResultCode(room, turn);
            if (room.ResultCode != null)
            {
                UpdateRoomResultCode((int)room.Id, (int)room.ResultCode);
            }
            _turnBLL.AddTurnToRoom(turn, (int)room.Id);
            return room;
        }

        private static int? DetermineResultCode(IRoomModel room, ITurnModel turn)
        {
            if (turn == null)
            {
                return null;
            }
            if (room.CheckForWin(turn))
            {
                return room.GetPlayerNum(turn.Num);
            }
            else if (room.Turns.Count >= room.Board.GetLength(0) * room.Board.GetLength(1))
            {
                return 0;
            }
            return null;
        }

        public int InsertNewRoom()
        {
            return _repository.InsertNewRoom();
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

        public IRoomModel UpdateWithLastTurn(IRoomModel room)
        {
            ITurnModel lastTurn = _turnBLL.GetLastTurnInRoom((int)room.Id);

            if (lastTurn == null && room.LocalPlayerNum == 1)
            {
                room.Message = "Where would you like to place a piece?";
                return room;
            }
            if (lastTurn?.Num == room.CurrentTurnNum)
            {
                room.Turns.Add(lastTurn);
                room.CurrentTurnNum++;
            }
            if (room.CurrentPlayerNum != room.LocalPlayerNum)
            {
                room.Message =
                    $"Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
                return room;
            }

            room.ResultCode = DetermineResultCode(room, lastTurn);
            if (room.ResultCode == null)
            {
                room.Message = "Where would you like to place a piece?";
            }

            return room;
        }

        private static string DetermineWinner(int? resultCode, Dictionary<int, string> players)
        {
            string winnerName = string.Empty;
            if (resultCode > 0 && resultCode < 3)
            {
                winnerName = $"{players[(int)resultCode]}";
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

        public IRoomModel AddPlayerToRoom(string localPlayerName, int roomId)
        {
            IRoomModel roomModel = GetRoomById(roomId);
            if (roomModel == null)
            {
                throw new ArgumentException($"Room Id {roomId} does not match any open rooms.");
            }
            if (roomModel.ResultCode != null)
            {
                throw new ArgumentException($"Room Id {roomId} is already finished!");
            }
            if (!roomModel.Vacancy)
            {
                throw new ArgumentException($"Room Id {roomId} is full!");
            }

            IPlayerModel playerModel = new PlayerModel { Name = localPlayerName };
            int playerNum;
            if (roomModel.Players[0] == null && roomModel.Players[1] == null)
            {
                playerNum = new Random().Next(1, 3);
            }
            else
            {
                playerNum = (roomModel.Players[0] == null) ? 1 : 2;
                string opponentName = roomModel.Players[2 - playerNum].Name;
                roomModel.Message = $"Successfully joined room against {opponentName}";
            }
            playerModel.Num = playerNum;
            playerModel = _playerBLL.AddPlayerToRoom(playerModel, (int)roomModel.Id);
            roomModel.LocalPlayerNum = playerNum;
            roomModel.Players[playerModel.Num - 1] = playerModel;
            return roomModel;
        }

        public IRoomModel GetRoomById(int roomId)
        {
            RoomDTO dto = _repository.GetRoomById(roomId);
            RoomModel room = ConvertToModel(dto);
            if (room == null)
            {
                return null;
            }
            room.Players = _playerBLL.GetPlayersInRoom(roomId);
            room.Vacancy = room.Players.Contains(null);

            return room;
        }

        public virtual IRoomModel LetThemPlay(IRoomModel roomModel)
        {
            bool isWaiting = true;
            while (isWaiting)
            {
                int turnNum = roomModel.Turns.Count;
                UpdateWithLastTurn(roomModel);

                if (roomModel.ResultCode != null)
                {
                    return roomModel;
                }

                if (turnNum == roomModel.Turns.Count)
                {
                    Thread.Sleep(2000);
                }

                if (roomModel.CurrentPlayerNum == roomModel.LocalPlayerNum)
                {
                    isWaiting = false;
                }
            }
            return roomModel;
        }

        private void UpdateRoomResultCode(int roomId, int resultCode)
        {
            _repository.UpdateRoomResultCode(roomId, resultCode);
        }

        internal RoomDTO ConvertToDto(IRoomModel model)
        {
            throw new NotImplementedException();
        }

        internal RoomModel ConvertToModel(RoomDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            RoomModel rM = new RoomModel();
            rM.Id = dto.Id;
            rM.CreationTime = dto.CreationTime;
            rM.CurrentTurnNum = dto.CurrentTurnNumber;
            rM.ResultCode = dto.ResultCode;
            return rM;
        }
    }
}
