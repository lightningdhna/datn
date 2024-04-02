using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.EmployeeModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.JobModels
{
    [Table("Stages")]
    public class OrderStage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StageIndex { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Order? Order { get; set; } = null;


        public List<Employee> InChargeTeam { get; set; } = [];

        [MaxLength(100)]
        public string? Role { get; set; }

        public StageStatus Status { get; set; }
    }
}
