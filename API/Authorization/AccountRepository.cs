using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API.Authorization
{


    public class AccountRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
            : IAccountRepository
    {
        public async Task EnsureRole(string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            var user = new IdentityUser
            {
                UserName = signUpModel.Username,

            };
            return await userManager.CreateAsync(user, signUpModel.Password);
        }

        private async Task<IdentityUser?> CheckSignIn(SignInModel signInModel)
        {
            var user = await userManager.FindByNameAsync(signInModel.Username);
            if (user == null)
            {
                return null;
            }
            var passwordCheck = await userManager.CheckPasswordAsync(user, signInModel.Password);
            if (!passwordCheck)
                return null;
            return user;
        }

        private async Task<List<Claim>> CreateClaims(IdentityUser user)
        {
            var authenticationClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await userManager.GetRolesAsync(user);
            authenticationClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            return authenticationClaims;
        }

        public async Task<string> SignInAsync(SignInModel signInModel)
        {

            var user = await userManager.FindByNameAsync(signInModel.Username);
            if (user == null)
            {
                throw new AuthException(AuthError.WrongUsername);
            }
            var passwordValid = await userManager.CheckPasswordAsync(user, signInModel.Password);
            if (!passwordValid)
            {
                throw new AuthException(AuthError.WrongPassword);
            }

            var authenticationClaims = await CreateClaims(user);

            var authenticationKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: authenticationClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(authenticationKey, SecurityAlgorithms.HmacSha512Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //public async Task<(string AccessToken, string RefreshToken)> SignInAsync(SignInModel signInModel)
        //{
        //    var user = await CheckSignIn(signInModel);
        //    if (user == null)
        //        return (string.Empty, string.Empty);

        //    var authenticationClaims = await CreateClaims(user);

        //    var authenticationKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
        //    var token = new JwtSecurityToken(
        //        issuer: configuration["Jwt:Issuer"],
        //        audience: configuration["Jwt:Audience"],
        //        claims: authenticationClaims,
        //        expires: DateTime.Now.AddMinutes(15), // Short-lived access token
        //        signingCredentials: new SigningCredentials(authenticationKey, SecurityAlgorithms.HmacSha512Signature)
        //    );

        //    var refreshToken = GenerateRefreshToken(); // Implement this method to generate a secure random token

        //    // Store the refresh token in your database here

        //    return (new JwtSecurityTokenHandler().WriteToken(token), refreshToken);
        //}
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }




        public async Task AddRoleToUser(string username, string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return;
            await EnsureRole(role);
            await userManager.AddToRoleAsync(user, role);
        }

        public async Task<List<string?>> GetAllRoles()
        {
            return await roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<IList<IdentityUser>> GetUserByRole(string role)
        {
            return await userManager.GetUsersInRoleAsync(role);
        }

        public async Task<IList<string>?> GetRoleForUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return null;
            return await userManager.GetRolesAsync(user);
        }

        public async Task<bool> HasRole(string username, string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return false;
            return await userManager.IsInRoleAsync(user, role);
        }
        public async Task<bool> UsernameExisted(string username)
        {
            return await userManager.FindByNameAsync(username) != null;
        }
    }
}
