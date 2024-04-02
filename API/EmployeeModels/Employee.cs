using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.JobModels;

namespace API.EmployeeModels
{
    [Table("Employees")]
    public class Employee
    {
        [NotMapped]
        private string _employeeId = "";

        [Key]
        [MaxLength(16)]
        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                if (string.IsNullOrEmpty(_employeeId))
                {
                    _employeeId = value;
                }
            }
        }

        [MaxLength(256)] public string LastName { get; set; } = "";
        [MaxLength(256)] public string FirstName { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";

        public List<OrderStage> JoinStages { get; set; } = [];



    }
}
