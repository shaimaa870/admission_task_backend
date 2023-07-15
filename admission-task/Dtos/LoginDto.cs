using System.ComponentModel.DataAnnotations;

namespace admission_task.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
    public class LoginResponse
    { 
        public string? Token { get; set; }
    }
}
