namespace ConnectFour.Data.DTOs
{
    public class PlayerDTO
    {
        public int? Id { get; set; }

        public int RoomId { get; set; }

        public string Name { get; set; } = string.Empty;
        public int Num { get; set; }
    }
}
