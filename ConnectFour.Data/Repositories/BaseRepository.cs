using System.Configuration;

using ConnectFour.Data.DALs;

namespace ConnectFour.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected static readonly IDAL _dal = new DAL(
            ConfigurationManager.ConnectionStrings["ConnectFourData"].ConnectionString
        );
    }
}
