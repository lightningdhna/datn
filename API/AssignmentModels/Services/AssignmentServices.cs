using API.EmployeeModels;
using API.JobModels;
using Microsoft.EntityFrameworkCore;

namespace API.AssignmentModels.Services
{


    public class AssignmentService(MyDbContext context) : IAssignmentService
    {
        public async Task AssignTaskAsync( OrderStage stage, Employee employee)
        {
            var assignment = new Assignment
            {
                Employee = employee,
                Stage = stage,
                OrderId = stage.OrderId,
                StageId = stage.StageIndex,
                EmployeeId = employee.EmployeeId
            };
            await context.AssignmentList.AddAsync(assignment);
            await context.SaveChangesAsync();
        }

        public async Task AssignTaskAsync( int orderId, int stageId, string employeeId)
        {
            var assignment = new Assignment
            {
                EmployeeId = employeeId,
                StageId = stageId,
                OrderId = orderId
            };
            await context.AssignmentList.AddAsync(assignment);
            await context.SaveChangesAsync();
        }


        public async Task DeleteAssignmentAsync(int orderId, int stageId, string employeeId)
        {
            var assignment = await context.AssignmentList
                .FirstOrDefaultAsync(
                    assignment => 
                        assignment.EmployeeId.Equals(employeeId)
                        && assignment.StageId == stageId
                        && assignment.OrderId == orderId
                        );
            if (assignment == null)
            {
                return;
            }

            context.AssignmentList.Remove(assignment);
            await context.SaveChangesAsync();
        }

        public async Task<List<Assignment>> GetAssignmentsAsync(string employeeId)
        {
            return await context.AssignmentList
                .Where(stage => stage.EmployeeId.Equals(employeeId))
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetAssignmentsForOrderAsync(int orderId)
        {
            var assignments = await context.AssignmentList
                .Where(a => a.OrderId == orderId)
                .ToListAsync();

            return assignments;
        }


        public async Task<List<OrderStage>> GetInProgressStagesAsync(int orderId)
        {
            var assignmentStages = await context.AssignmentList
                .Where(a => a.OrderId == orderId)
                .Select(a => new { a.OrderId, a.StageId })
                .Distinct()
                .ToListAsync();

            var stages = await context.StageList
                .Where(s => assignmentStages.Any(a => a.OrderId == s.OrderId && a.StageId == s.StageIndex))
                .ToListAsync();

            return stages;
        }
        public async Task<List<int>> GetInProgressStageIdsAsync(int orderId)
        {
            var stageIds = await context.AssignmentList
                .Where(a => a.OrderId == orderId)
                .Select(a => a.StageId)
                .Distinct()
                .ToListAsync();

            return stageIds;
        }
        public async Task<bool> AssignmentExistsAsync(int orderId, int stageId, string employeeId)
        {
            return await context.AssignmentList
                .AnyAsync(a => a.OrderId == orderId && a.StageId == stageId && a.EmployeeId.Equals(employeeId));
        }




    }

}
