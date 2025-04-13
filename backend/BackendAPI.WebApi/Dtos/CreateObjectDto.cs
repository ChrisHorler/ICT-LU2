using System.ComponentModel.DataAnnotations;

namespace BackendAPI.WebApi.Dtos {
    public class CreateObjectDto {
        
        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        public int PositionX { get; set; }
        
        [Required]
        public int PositionY { get; set; }
        
        [Required]
        public float Rotation { get; set; }
        
        [Required]
        public float Scale { get; set; }
    }
}