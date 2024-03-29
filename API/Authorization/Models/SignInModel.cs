using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Authorization.Models
{
    public class SignInModel
    {
        [Required] public string Username { get; set; } = null!;

        [Required] public string Password { get; set; } = null!;

    }
}
