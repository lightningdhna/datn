using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
       
        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        public string Get()
        {
            return "1";
        }
    }
}
