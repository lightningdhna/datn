using Microsoft.EntityFrameworkCore;

namespace API.JobModels.Services
{
    public class StageService(MyDbContext context) : IStageService
    {
        public async Task CreateStage( Order order, OrderStage stage)
        {
            stage.StageIndex = 0;
            stage.Order = order;
            context.StageList.Add(stage);
            await context.SaveChangesAsync();
        }

        public async Task CreateStage(int orderId, OrderStage stage)
        {
            var order = await context.OrderList.FindAsync(orderId) ;
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            await CreateStage(order, stage);
        }

        public async Task UpdateStage( OrderStage stage)
        {
            context.StageList.Update(stage);
            await context.SaveChangesAsync();
        }

        public async Task DeleteStage(int stageId )
        {
            var stage = await context.StageList.FindAsync(stageId);
            if (stage != null)
            {
                context.StageList.Remove(stage);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<OrderStage>> GetStagesForOrder(int orderId)
        {
            return await context.StageList
                .Where(s => s.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrderStage?> GetStageById(int orderId, int stageIndex)
        {
            return await context.StageList
                .Where(s => s.OrderId== orderId && s.StageIndex == stageIndex)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> StageExistsAsync(int orderId, int stageIndex)
        {
            return await context.StageList
                .AnyAsync(s => s.OrderId == orderId && s.StageIndex == stageIndex);
        }

    }
}
