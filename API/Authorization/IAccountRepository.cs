using API.Authorization.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Authorization;

public interface IAccountRepository
{
    Task EnsureRole(string role);
    Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
    Task AddRoleToUser(string username, string role);
    Task<List<string?>> GetAllRoles();
    Task<IList<IdentityUser>> GetUserByRole(string role);
    Task<IList<string>?> GetRoleForUser(string username);
    Task<bool> HasRole(string username,string role);
    Task<bool> UsernameExisted(string username);
    Task<string> SignInAsync(SignInModel signInModel);
    //public Task<(string AccessToken, string RefreshToken)> SignInAsync(SignInModel signInModel);

}