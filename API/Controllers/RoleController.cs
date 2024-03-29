using System.Security.Claims;
using API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IAccountRepository accountRepo) : ControllerBase
    {
        [HttpPost("{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNewRole(string role)
        {
            await accountRepo.EnsureRole(role);
            return Ok();
        }

        [HttpGet("of/{username}")]
        public async Task<IActionResult> GetRolesOfUser(string username)
        {
            var roles = await accountRepo.GetRoleForUser(username);
            if (roles == null || roles.Count == 0)
                return NotFound();
            return Ok(roles);
        }

        [HttpGet("all-roles")]
        [Authorize(Roles= "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await accountRepo.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("user-role")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if(username== null)
                return BadRequest();
            var roles =  await accountRepo.GetRoleForUser(username);
            if (roles ==null || roles.Count == 0)
                return NotFound();
            return Ok(roles);
        }
    }
}
