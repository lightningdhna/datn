using API.JobModels.Services;
using API.JobModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StageController(IStageService stageService) : ControllerBase
    {


        [HttpPost("{orderId}")]
        public async Task<IActionResult> CreateStage(int orderId, [FromBody] OrderStage stage)
        {
            await stageService.CreateStage(orderId, stage);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStage([FromBody] OrderStage stage)
        {
            await stageService.UpdateStage(stage);
            return Ok();
        }

        [HttpDelete("{stageId}")]
        public async Task<IActionResult> DeleteStage(int stageId)
        {
            await stageService.DeleteStage(stageId);
            return Ok();
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetStagesForOrder(int orderId)
        {
            var stages = await stageService.GetStagesForOrder(orderId);
            return Ok(stages);
        }
        [HttpGet("by-id/{orderId}/{stageId}")]
        public async Task<IActionResult> GetStageById(int orderId, int stageId)
        {
            var stage = await stageService.GetStageById(orderId, stageId);
            return Ok(stage);
        }
    }

}
