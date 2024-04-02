namespace API.EmployeeModels;

public interface IEmployeeService
{
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(string employeeId);
    Task<Employee?> GetEmployeeAsync(string employeeId);
    public Task<bool> EmployeeExistsAsync(string employeeId);
}