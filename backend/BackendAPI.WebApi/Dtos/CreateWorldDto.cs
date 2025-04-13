using System.ComponentModel.DataAnnotations;

namespace BackendAPI.WebApi.Dtos {
    public class CreateWorldDto {
        
        [Required]
        [MaxLength(25)]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int SizeX { get; set; }
        
        [Required]
        public int SizeY { get; set; }
    }
}