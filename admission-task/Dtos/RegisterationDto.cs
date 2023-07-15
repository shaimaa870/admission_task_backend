using System.ComponentModel.DataAnnotations;

namespace admission_task.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string? Name { get; set; }
        public List<string>? Roles { get; set; }

    }
}
