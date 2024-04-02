using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.EmployeeModels;
using API.JobModels;

namespace API.AssignmentModels
{
    [Table("Assignments")]
    public class Assignment
    {
        [ForeignKey("Stages")]
        public required int OrderId { get; set; }

        [ForeignKey("Stages")]
        public required int StageId { get; set; }

        [ForeignKey("Employees")]
        [MaxLength(16)]
        public required string EmployeeId { get; set; }

        public Employee? Employee { get; set; }
        public OrderStage? Stage { get; set; }

    }
}
