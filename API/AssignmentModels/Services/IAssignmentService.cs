using API.EmployeeModels;
using API.JobModels;

namespace API.AssignmentModels.Services;

public interface IAssignmentService
{
    Task AssignTaskAsync(OrderStage stage, Employee employee);
    Task AssignTaskAsync(int orderId, int stageId, string employeeId);
    Task DeleteAssignmentAsync(int orderId, int stageId, string employeeId);
    Task<List<Assignment>> GetAssignmentsAsync(string employeeId);
    Task<List<Assignment>> GetAssignmentsForOrderAsync(int orderId);
    Task<List<OrderStage>> GetInProgressStagesAsync(int orderId);
    Task<List<int>> GetInProgressStageIdsAsync(int orderId);
    public Task<bool> AssignmentExistsAsync(int orderId, int stageId, string employeeId);
}