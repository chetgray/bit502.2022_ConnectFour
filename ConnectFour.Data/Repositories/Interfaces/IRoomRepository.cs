using ConnectFour.Data.DTOs;
using System.Collections.Generic;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        List<ResultDTO> GetAllFinished();
    }
}
