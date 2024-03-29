using API.JobModels;
using API.UserModels;

namespace API.Services.RoleManagement
{
    public interface IRoleManager
    {
        public Task<Role> AddRole(Role role);
        public Task<IEnumerable<Role>> GetRoleAsync(Guid userId);

    }
}
