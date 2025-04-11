namespace BackendAPI.WebApi.Dtos {
    public class CreateObjectDto {
        public string Type { get; set; } = string.Empty;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
    }
}