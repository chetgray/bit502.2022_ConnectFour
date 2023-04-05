using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Business.BLLs
{
    public class NPCRoomBLL : RoomBLL, IRoomBLL
    {
        /// <summary>
        /// Creates an <see cref="NPCRoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public NPCRoomBLL()
        {
        }

        /// <summary>
        /// Creates an <see cref="NPCRoomBLL"/> instance with the passed <paramref name="repository"/>,
        /// <paramref name="playerBLL"/>, and <paramref name="turnBLL"/> as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use in the backend.</param>
        /// <param name="playerBLL">The <see cref="IPlayerBLL"/> to use in the backend.</param>
        /// <param name="turnBLL">The <see cref="ITurnBLL"/> to use in the backend.</param>
        public NPCRoomBLL(IRoomRepository repository, IPlayerBLL playerBLL, ITurnBLL turnBLL) : base(repository, playerBLL, turnBLL)
        {
        }
        public override IRoomModel LetThemPlay(IRoomModel room)
        {
            ITurnModel turn = RandyTakesATurn(room);
            room = AddTurnToRoom(turn, room);
            room.Message = "Where would you like to place a piece?";
            return room;
        }

        public override IRoomModel AddPlayerToRoom(string localPlayerName, int roomId)
        {
            IRoomModel room = base.AddPlayerToRoom("Randy", roomId);
            room = base.AddPlayerToRoom(localPlayerName, roomId);
            return room;
        }

        public List<(int, int)> GetValidPlays(IRoomModel room)
        {
            List<(int, int)> validPlays = new List<(int, int)>();

            for (int i = 1; i <= 7; i++)
            {
                try
                {
                    int rowNum = room.GetNextRowInCol(i);
                    validPlays.Add((rowNum, i));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }

            return validPlays;
        }

        public TurnModel RandyTakesATurn(IRoomModel room)
        {
            List<(int, int)> validPlays = GetValidPlays(room);
            Random random = new Random();
            (int rowNum, int colNum) play = validPlays.ElementAt(random.Next(0, validPlays.Count));
            TurnModel turn = new TurnModel
            {
                ColNum = play.colNum,
                RowNum = play.rowNum,
                Num = room.CurrentTurnNum
            };

            return turn;
        }

        /// <summary>
        /// Opportunistic Ophelia takes a turn. She will try to win, but if she can't, she will
        /// try to block the player from winning. If she can't do either of those things, she
        /// will play randomly.
        /// </summary>
        /// <param name="room"></param>
        /// <returns>
        /// A <see cref="TurnModel"/> with the AI's play.
        /// </returns>
        public TurnModel OpheliaTakesATurn(IRoomModel room)
        {
            Random random = new Random();
            List<(int, int)> validPlays = GetValidPlays(room);

            // Try to win
            List<TurnModel> winningTurns = new List<TurnModel>();
            foreach ((int, int) play in validPlays)
            {
                TurnModel turn = new TurnModel
                {
                    ColNum = play.Item2,
                    RowNum = play.Item1,
                    Num = room.CurrentTurnNum
                };
                if (room.CheckForWin(turn))
                {
                    winningTurns.Add(turn);
                }
            }
            if (winningTurns.Any())
            {
                return winningTurns[random.Next(0, winningTurns.Count)];
            }

            // Try to block the player from winning
            List<TurnModel> blockingTurns = new List<TurnModel>();
            foreach ((int, int) play in validPlays)
            {
                TurnModel turn = new TurnModel
                {
                    ColNum = play.Item2,
                    RowNum = play.Item1,
                    Num = room.CurrentTurnNum + 1
                };
                if (room.CheckForWin(turn))
                {
                    turn.Num = room.CurrentTurnNum;
                    blockingTurns.Add(turn);
                }
            }
            if (blockingTurns.Any())
            {
                return blockingTurns[random.Next(0, blockingTurns.Count)];
            }

            // Play randomly
            (int rowNum, int colNum) = validPlays[random.Next(0, validPlays.Count)];
            TurnModel randomTurn = new TurnModel
            {
                ColNum = colNum,
                RowNum = rowNum,
                Num = room.CurrentTurnNum
            };
            return randomTurn;
        }
    }
}
