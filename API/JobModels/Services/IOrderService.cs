namespace API.JobModels.Services;

public interface IOrderService
{
    Task<int> CreateOrder(Order order);
    Task<Order?> GetOrder(int orderId);
    Task<int> UpdateOrder(Order order);
    Task DeleteOrder(int orderId);
    public Task<bool> OrderExistsAsync(int orderId);
}