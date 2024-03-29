using API.JobModels;
using API.UserModels;

namespace API.Services.JobManagement
{
    public interface IJobManager
    {
        public Task<Job> GetJobAsync(User user, Guid id);
        public Task<IEnumerable<Job>> GetJobForUserAsync(User user);

        public Task<IEnumerable<Job>> GetJobForRoleAsync(Role role);

        public Task<Job> AddJobForUser(User user, Job job);
        public Task<Job> CreateJob(Job job);
        public Task<Job> UpdateJob(Job job);
        public Task<Job> DeleteJob(Job job);

    }
}
