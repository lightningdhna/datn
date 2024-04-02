namespace API.JobModels.Services;

public interface IStageService
{
    Task CreateStage( Order order, OrderStage stage);
    Task CreateStage(int orderId, OrderStage stage);
    Task UpdateStage( OrderStage stage);
    Task DeleteStage(int stageId );
    Task<List<OrderStage>> GetStagesForOrder(int orderId);
    public Task<OrderStage?> GetStageById(int orderId, int stageIndex);
    public  Task<bool> StageExistsAsync(int orderId, int stageIndex);


}