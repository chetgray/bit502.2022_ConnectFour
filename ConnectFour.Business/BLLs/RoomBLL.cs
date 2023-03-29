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

        public IRoomModel AddTurnToRoom(int colNum, IRoomModel room)
        {
            if (room.CurrentPlayerNum != room.LocalPlayerNum)
            {
                room.Message =
                    $"It's Not Your Turn! Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
                return room;
            }

            if (colNum < 1 || colNum > 7)
            {
                room.Message = "Please choose a column between 1 - 7";
                return room;
            }

            int turnsInColumnCount = 0;
            foreach (TurnModel turnModel in room.Turns)
            {
                if (turnModel.ColNum == colNum)
                {
                    turnsInColumnCount++;
                }
            }
            int rowNum = room.Board.GetLength(0) - turnsInColumnCount;
            if (rowNum < 1)
            {
                room.Message = "That Column is Full!";
                return room;
            }

            TurnBLL turnBLL = new TurnBLL();
            TurnModel turn = new TurnModel
            {
                ColNum = colNum,
                RowNum = rowNum,
                Num = (int)room.CurrentTurnNum
            };

            room.Turns.Add(turn);
            turnBLL.AddTurnToRoom(turn, (int)room.Id);
            room.CurrentTurnNum++;

            if (room.CheckForWin(turn))
            {
                room.ResultCode = room.GetPlayerNum(turn.Num);
                UpdateRoomResultCode((int)room.Id, (int)room.ResultCode);
            }

            int totalPiecesToPlay = room.Board.GetLength(0) * room.Board.GetLength(1);
            if (room.Turns.Count >= totalPiecesToPlay)
            {
                room.ResultCode = 0;
                return room;
            }

            room.Message =
                $"Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
            return room;
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

        public IRoomModel GetLastTurnInRoom(IRoomModel room)
        {
            ITurnModel turn = _turnBLL.GetLastTurnInRoom((int)room.Id);

            if (room.CurrentTurnNum == null)
            {
                room.CurrentTurnNum = 1;
            }

            if (turn == null)
            {
                if (room.LocalPlayerNum == 1)
                {
                    room.Message = "Where would you like to place a piece?";
                    return room;
                }
                else
                {
                    room.Message =
                        $"Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
                    return room;
                }
            }

            if (room.Turns.Count != turn.Num)
            {
                if (room.CheckForWin(turn))
                {
                    room.ResultCode = room.GetPlayerNum(turn.Num);
                }
                else
                {
                    room.Message = "Where would you like to place a piece?";
                }
                room.Board[turn.RowNum - 1, turn.ColNum - 1] = room.CurrentPlayerNum;
                room.CurrentTurnNum++;
                room.Turns.Add(turn);
            }
            else if (room.Turns.Count == turn.Num)
            {
                room.Message =
                    $"Waiting on {room.Players[room.CurrentPlayerNum - 1].Name} to place a piece.";
                return room;
            }

            int totalPiecesToPlay = room.Board.GetLength(0) * room.Board.GetLength(1);
            if (room.Turns.Count >= totalPiecesToPlay)
            {
                room.ResultCode = 0;
                room.Message = string.Empty;
                return room;
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
                GetLastTurnInRoom(roomModel);

                if (roomModel.ResultCode != null)
                {
                    return roomModel;
                }

                if (turnNum == roomModel.Turns.Count)
                {
                    Thread.Sleep(5000);
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
