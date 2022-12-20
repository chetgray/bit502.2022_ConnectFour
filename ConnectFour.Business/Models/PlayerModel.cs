using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    internal class PlayerModel : IPlayerModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
