using Microsoft.EntityFrameworkCore;

namespace API.CacheMemoryService
{
    public interface ICacheQueryService
    {
        Task<string?> GetEmployeeIdAsync(string username);
        Task<T?> GetEntityAsync<T>(params object[] keyValues) where T : class;
    }

    public class CacheQueryService(IRedisCache redis, MyDbContext context) : ICacheQueryService
    {

        public async Task<string?> GetEmployeeIdAsync(string username)
        {
            var cacheKey = $"EmployeeId:{username}";
            var employeeId = await redis.GetAsync<string>(cacheKey);
            if (employeeId != null) return employeeId;

            var employee = await context.EmployeeList.FirstOrDefaultAsync(e => e.Username == username);
            if (employee == null) return null;

            employeeId = employee.EmployeeId;
            await redis.SetAsync(cacheKey, employeeId, TimeSpan.FromHours(12), TimeSpan.FromMinutes(30));

            return employeeId;
        }

        public async Task<T?> GetEntityAsync<T>(params object[] keyValues) where T : class
        {
            var cacheKey = $"Entity:{typeof(T).Name}:{string.Join(":", keyValues)}";
            var entity = await redis.GetAsync<T>(cacheKey);
            if (entity != null) return entity;

            entity = await context.Set<T>().FindAsync(keyValues);
            if (entity != null)
            {
                await redis.SetAsync(cacheKey, entity, TimeSpan.FromHours(1), TimeSpan.FromMinutes(30));
            }

            return entity;
        }



    }
}
