namespace ConnectFour.Data.DTOs
{
    public class PlayerDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int RoomId { get; set; }
        public int Num { get; set; }
    }
}
