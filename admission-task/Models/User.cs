using Microsoft.AspNetCore.Identity;

namespace admission_task.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
    }
}
