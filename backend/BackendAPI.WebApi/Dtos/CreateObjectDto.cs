using System.ComponentModel.DataAnnotations;

namespace BackendAPI.WebApi.Dtos {
    public class CreateObjectDto {
        
        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        public float PositionX { get; set; }
        
        [Required]
        public float PositionY { get; set; }
        
        [Required]
        public float Rotation { get; set; }
        
        [Required]
        public float Scale { get; set; }
    }
}