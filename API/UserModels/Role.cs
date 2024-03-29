using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.SignalR;

namespace API.UserModels
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [MaxLength(256)]
        public String Name { get; set; } = "";


        public static readonly Role DefaultRole = new Role { Name = "Default" };
    }
}
