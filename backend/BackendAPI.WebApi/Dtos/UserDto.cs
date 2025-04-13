using System.ComponentModel.DataAnnotations;

namespace BackendAPI.WebApi.Dtos
{
    public class UserDto
    { 
        [Required]
        public string Username { get; set; } = string.Empty; 
        
        [Required]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_\-+=])[A-Za-z\d!@#$%^&*()_\-+=]{10,}$",
            ErrorMessage = "Password must be at least 10 characters, and include uppercase, lowercase, number, and special character."
        )]
        public string Password { get; set; } = string.Empty;
    }  
}

