using API.UserModels;

namespace API.Services.UserManagement
{
    public interface IUserManager
    {
        public Task<User> GetUserAsync(Guid userId);
        public Role GetRole(Guid userId);
        public Task<User> CreateUserAsync(User user);
        public Task<User> UpdateUserAsync(Guid userId, User user);

        public Task<User> GetUserByRoleAsync(Role role);

        }
}
