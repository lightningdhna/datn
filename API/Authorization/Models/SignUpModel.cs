using System.ComponentModel.DataAnnotations;

namespace API.Authorization.Models
{
    public class SignUpModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required] public string Password { get; set; } = null!;
    }
}
