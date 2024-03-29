using API.Authorization;
using API.Authorization.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountRepository repo) : ControllerBase
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            if (await repo.UsernameExisted(signUpModel.Username))
            {
                return BadRequest("Username existed");
            }
            var result = await repo.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var token = await repo.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
