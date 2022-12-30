using System;
using System.Collections.Generic;
using System.Data;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class RoomRepository : BaseRepository
    {
        public List<ResultDTO> GetAllFinished()
        {
            ResultDTO resultDTO = new ResultDTO();
            resultDTO.CreationTime = DateTime.Now;
            resultDTO.CurrentTurnNum = 11;
            resultDTO.Players = new List<PlayerDTO>
            {
                new PlayerDTO
                {
                    Id = 1,
                    Name = "John",
                    Num = 1,
                    RoomId = 1
                },
                new PlayerDTO
                {
                    Id = 2,
                    Name = "Bob",
                    Num = 2,
                    RoomId = 1
                }
            };
            resultDTO.ResultCode = 1;
            resultDTO.RoomId = 1;
            resultDTO.Turns = new List<TurnDTO>
            {
                new TurnDTO
                {
                    Id = 1,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 1
                },
                new TurnDTO
                {
                    Id = 2,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 2
                },
                new TurnDTO
                {
                    Id = 3,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 3
                },
                new TurnDTO
                {
                    Id = 4,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 4
                },
                new TurnDTO
                {
                    Id=5,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 5
                },
                new TurnDTO
                {
                    Id = 6,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 6
                },
                new TurnDTO
                {
                    Id = 7,
                    Time = DateTime.Now,
                    RowNum = 1,
                    ColNum = 1,
                    RoomId = 1,
                    Num = 7
                }
            };
            List<ResultDTO> resultDTOS = new List<ResultDTO>();
            resultDTOS.Add(resultDTO);
            return resultDTOS;
        }
        private static RoomDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
