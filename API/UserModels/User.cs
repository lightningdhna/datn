using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.UserModels
{
    [Table("User")]
    public class User : IUser
    {

        [Key]
        public Guid Id { get; init; }
        [Required]
        public Role UserRole { get; set; } = Role.DefaultRole;
 
    }
}
