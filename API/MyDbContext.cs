
using API.AssignmentModels;
using API.Authorization;
using API.EmployeeModels;
using API.JobModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {

        public required DbSet<Employee> EmployeeList { get; set; }

        public required DbSet<Order> OrderList { get; set; }
        public required DbSet<OrderStage> StageList { get; set; }

        public required DbSet<Assignment> AssignmentList { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderStage>()
                .HasKey(s => new { s.OrderId, StageId = s.StageIndex });
            builder.Entity<Assignment>()
                .HasOne(a => a.Stage)
                .WithMany()
                .HasForeignKey(a => new { a.OrderId, a.StageId })
                .HasPrincipalKey(s => new { s.OrderId, StageId = s.StageIndex });


            builder.Entity<Assignment>()

                .HasKey(a => new { a.OrderId,a.StageId, a.EmployeeId })
                ;
        }

    }
}
