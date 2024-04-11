using Microsoft.EntityFrameworkCore;

namespace API.JobModels.Services
{
    public class OrderService(MyDbContext context) : IOrderService
    {

        // Order operations
        public async Task<int> CreateOrder(Order order)
        {
            order.OrderId = 0;
            context.OrderList.Add(order);
            return await context.SaveChangesAsync();
        }

        public async Task<Order?> GetOrder(int orderId)
        {
            return await context.OrderList.FindAsync(orderId);
        }

        public async Task<int> UpdateOrder(Order order)
        {
            context.OrderList.Update(order);
            return await context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await context.OrderList.FindAsync(orderId);
            if (order != null)
            {
                context.OrderList.Remove(order);
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> OrderExistsAsync(int orderId)
        {
            //return await context.OrderList
            //    .AnyAsync(o => o.OrderId == orderId);
            var order = await context.OrderList.FindAsync(orderId);
            return order != null;
        }




    }

}
