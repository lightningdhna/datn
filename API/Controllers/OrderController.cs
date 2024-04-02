using API.JobModels;
using API.JobModels.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderServices) : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await orderServices.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await orderServices.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            if(! await orderServices.OrderExistsAsync(id))
            {
                return NotFound();
            }
            await orderServices.UpdateOrder(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!await orderServices.OrderExistsAsync(id))
            {
                return NotFound();
            }
            await orderServices.DeleteOrder(id);
            return NoContent();
        }


    }
}
