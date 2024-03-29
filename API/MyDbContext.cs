
using API.Authorization;
using API.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {

        //public required DbSet<User> UserList { get; set; }


    }
}
