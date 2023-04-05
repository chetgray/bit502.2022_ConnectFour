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
        protected readonly IPlayerBLL _playerBll;
        protected readonly ITurnBLL _turnBll;

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public RoomBLL()
        {
            _repository = new RoomRepository();
            _playerBll = new PlayerBLL();
            _turnBll = new TurnBLL();
        }

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with the passed <paramref name="repository"/>,
        /// <paramref name="playerBll"/>, and <paramref name="turnBll"/> as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use in the backend.</param>
        /// <param name="playerBll">The <see cref="IPlayerBLL"/> to use in the backend.</param>
        /// <param name="turnBll">The <see cref="ITurnBLL"/> to use in the backend.</param>
        public RoomBLL(IRoomRepository repository, IPlayerBLL playerBll, ITurnBLL turnBll)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _playerBll = playerBll ?? throw new ArgumentNullException(nameof(playerBll));
            _turnBll = turnBll ?? throw new ArgumentNullException(nameof(turnBll));
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

            ITurnModel turn = new TurnModel
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
                SetRoomResultCode((int)room.Id, (int)room.ResultCode);
            }
            _turnBll.AddTurnToRoom(turn, (int)room.Id);
            return room;
        }

        private static int? DetermineResultCode(IRoomModel room, ITurnModel turn)
        {
            if (turn == null)
            {
                return null;
            }
            if (room.WillTurnWin(turn))
            {
                return room.DeterminePlayerNum(turn.Num);
            }
            else if (room.Turns.Count >= room.Board.GetLength(0) * room.Board.GetLength(1))
            {
                return 0;
            }
            return null;
        }

        public int AddNewRoom()
        {
            return _repository.AddNewRoom();
        }

        public List<IResultModel> GetAllFinished()
        {
            List<IResultModel> models = new List<IResultModel>();
            List<ResultDTO> dtos = _repository.GetAllFinishedResults();
            for (int i = 0; i < dtos.Count; i++)
            {
                models.Add(ConvertToResultModel(dtos[i]));
            }
            return models;
        }

        internal static IResultModel ConvertToResultModel(ResultDTO dto)
        {
            string[] playerNames = new string[dto.Players.Count];
            foreach (KeyValuePair<int, string> playerNumName in dto.Players)
            {
                playerNames[playerNumName.Key - 1] = playerNumName.Value;
            }
            TimeSpan durationTimeSpan =
                dto.LastTurnTime != null
                    ? (DateTime)dto.LastTurnTime - dto.CreationTime
                    : DateTime.Now - dto.CreationTime;
            IResultModel model = new ResultModel
            {
                RoomId = dto.RoomId,
                CreationTime = dto.CreationTime,
                Duration = durationTimeSpan,
                Players = playerNames,
                ResultCode = dto.ResultCode,
                WinnerName = DetermineWinner(dto.ResultCode, dto.Players),
                LastTurnNum = dto.LastTurnNum
            };
            return model;
        }

        public static IResultModel ConvertToResultModel(IRoomModel room)
        {
            Dictionary<int, string> playerNameDictionary = room.Players.ToDictionary(
                p => p.Num,
                p => p.Name
            );
            ResultDTO resultDto = new ResultDTO
            {
                RoomId = room.Id,
                CreationTime = room.CreationTime,
                Players = playerNameDictionary,
                ResultCode = room.ResultCode,
            };
            if (room.Turns.Any())
            {
                ITurnModel lastTurn = room.Turns.Last();
                resultDto.LastTurnTime = lastTurn.Time;
                resultDto.LastTurnNum = lastTurn.Num;
            }
            IResultModel resultModel = ConvertToResultModel(resultDto);
            return resultModel;
        }

        public IRoomModel UpdateWithLatestTurn(IRoomModel room)
        {
            ITurnModel lastTurn = _turnBll.GetLatestTurnInRoom((int)room.Id);

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
            if (resultCode > 0 && resultCode < 3)
            {
                return $"{players[(int)resultCode]}";
            }
            else if (resultCode == 0)
            {
                return "DRAW";
            }
            return "NULL";
        }

        public virtual IRoomModel AddPlayerToRoom(string localPlayerName, int roomId)
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
            playerModel = _playerBll.AddPlayerToRoom(playerModel, (int)roomModel.Id);
            roomModel.LocalPlayerNum = playerNum;
            roomModel.Players[playerModel.Num - 1] = playerModel;
            return roomModel;
        }

        public IRoomModel GetRoomById(int roomId)
        {
            RoomDTO dto = _repository.GetRoomById(roomId);
            IRoomModel room = ConvertToModel(dto);
            if (room == null)
            {
                return null;
            }
            room.Players = _playerBll.GetPlayersInRoom(roomId);
            room.Vacancy = room.Players.Contains(null);

            return room;
        }

        public virtual IRoomModel WaitForOpponentToPlay(IRoomModel roomModel)
        {
            bool isWaiting = true;
            while (isWaiting)
            {
                int turnNum = roomModel.Turns.Count;
                UpdateWithLatestTurn(roomModel);

                if (roomModel.ResultCode != null)
                {
                    return roomModel;
                }

                if (turnNum == roomModel.Turns.Count)
                {
                    Thread.Sleep(1000);
                }

                if (roomModel.CurrentPlayerNum == roomModel.LocalPlayerNum)
                {
                    isWaiting = false;
                }
            }
            return roomModel;
        }

        private void SetRoomResultCode(int roomId, int resultCode)
        {
            _repository.SetRoomResultCode(roomId, resultCode);
        }

        internal RoomDTO ConvertToDto(IRoomModel model)
        {
            throw new NotImplementedException();
        }

        internal IRoomModel ConvertToModel(RoomDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            IRoomModel model = new RoomModel()
            {
                Id = dto.Id,
                CreationTime = dto.CreationTime,
                CurrentTurnNum = dto.CurrentTurnNumber,
                ResultCode = dto.ResultCode
            };
            return model;
        }
    }
}
