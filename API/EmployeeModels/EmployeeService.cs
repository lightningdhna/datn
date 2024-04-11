using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.EmployeeModels;

namespace API.EmployeeModels
{
    public class EmployeeService(MyDbContext context) : IEmployeeService
    {
        public async Task AddEmployeeAsync(Employee employee)
        {
            var currentYear = DateTime.Now.Year;
            var lastEmployee = await context.EmployeeList
                .Where(e => e.EmployeeId!.StartsWith(currentYear.ToString()))
                .OrderByDescending(e => e.EmployeeId)
                .FirstOrDefaultAsync();

            var lastIndex = lastEmployee != null ? int.Parse(lastEmployee.EmployeeId![4..]) : 0;
            var nextIndex = lastIndex + 1;

            var nextId = $"{currentYear}{nextIndex:D4}";

            employee.EmployeeId = nextId;

            context.EmployeeList.Add(employee);
            await context.SaveChangesAsync();
        }


        public async Task UpdateEmployeeAsync(Employee employee)
        {
            context.EmployeeList.Update(employee);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(string employeeId)
        {
            var employee = await context.EmployeeList.FindAsync(employeeId);
            if (employee == null)
            {
                return;
                throw new ArgumentException("Employee not found");
            }

            context.EmployeeList.Remove(employee);
            await context.SaveChangesAsync();
        }

        public async Task<Employee?> GetEmployeeAsync(string employeeId)
        {
            return await context.EmployeeList.FindAsync(employeeId);
        }
        //public async Task<bool> EmployeeExistsAsync(string employeeId)
        //{
        //    return await context.EmployeeList
        //        .AnyAsync(e => e.EmployeeId.Equals(employeeId));
        //}
        public async Task<bool> EmployeeExistsAsync(string employeeId)
        {
            var employee = await context.EmployeeList.FindAsync(employeeId);
            return employee != null;
        }



    }
}