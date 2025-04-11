namespace BackendAPI.WebApi.Dtos {
    public class CreateWorldDto {
        public string Name { get; set; } = string.Empty;
        public int SizeX { get; set; }
        public int SizeY { get; set; }
    }
}