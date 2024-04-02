using API.Authorization;
using API.EmployeeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController(
        IAccountRepository repo, 
        IEmployeeService empService
    
        ) : ControllerBase
    {
       
        //[HttpGet(Name = "GetWeatherForecast")]
        //[Authorize]
        //public string Get()
        //{
        //    return "1";
        //}
        
        [HttpGet("/GetUsername")]

        public string GetUsername()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value!;
        }

        [HttpGet("/TestEmployeeServices")]
        public async Task<string> Test1()
        {
            Employee emp = new Employee()
            {
                FirstName = "Hưng",
                LastName = "Phạm Duy"
            };
            await empService.AddEmployeeAsync(emp);
            await empService.GetEmployeeAsync(emp.EmployeeId!);
            //Console.WriteLine(emp.EmployeeId);
            await empService.DeleteEmployeeAsync(emp.EmployeeId!);
            if (emp.EmployeeId == "20240001")
            {
                return "Test Passed";
            }
            else
            {
                return "Test Failed";
            }
        }
        [HttpGet("/TestDeleteEmployeeServices/{id}")]
        public async Task<string> TestRemoveEmployee(string id)
        {

            await empService.DeleteEmployeeAsync(id);
            var emp = await empService.GetEmployeeAsync(id);

            if (emp == null)
            {
                return "Test Passed";
            }
            else
            {
                return "Test Failed";
            }
        }

    }
}
