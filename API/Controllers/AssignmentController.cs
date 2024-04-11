using API.AssignmentModels.Services;
using API.EmployeeModels;
using API.JobModels.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController(
        IAssignmentService assignmentService,
        IEmployeeService employeeService,
        IStageService stageService,
        IOrderService orderService

    ) : ControllerBase
    {
        [HttpPost("/AssignTask/{orderId}/{stageIndex}/{employeeId}")]
        public async Task<IActionResult> AssignTaskAsync(int orderId, int stageIndex, string employeeId)
        {
            if (!await employeeService.EmployeeExistsAsync(employeeId))
            {
                return BadRequest("Employee not found");
            }

            if (!await stageService.StageExistsAsync(orderId, stageIndex))
            {
                return BadRequest("Stage not found");
            }
            await assignmentService.AssignTaskAsync(orderId, stageIndex, employeeId);
            return Ok();
        }

        [HttpDelete("/RemoveAssignment/{orderId}/{stageIndex}/{employeeId}")]
        public async Task<IActionResult> DeleteAssignmentAsync(int orderId, int stageIndex, string employeeId)
        {
            if (!await employeeService.EmployeeExistsAsync(employeeId))
            {
                return BadRequest("Employee not found");
            }

            if (!await stageService.StageExistsAsync(orderId, stageIndex))
            {
                return BadRequest("Stage not found");
            }
            await assignmentService.DeleteAssignmentAsync(orderId, stageIndex, employeeId);
            return Ok();
        }

        [HttpGet("GetAssignments/{employeeId}")]
        public async Task<IActionResult> GetAssignmentsAsync(string employeeId)
        {
            if (!await employeeService.EmployeeExistsAsync(employeeId))
            {
                return BadRequest("Employee not found");
            }

            var assignments = await assignmentService.GetAssignmentsAsync(employeeId);
            return Ok(assignments);
        }

        [HttpGet("GetAssignmentsOfOrder/{orderId}")]
        public async Task<IActionResult> GetAssignmentsForOrderAsync(int orderId)
        {
            if (!await orderService.OrderExistsAsync(orderId))
            {
                return BadRequest("Order not found");
            }
            var assignments = await assignmentService.GetAssignmentsForOrderAsync(orderId);
            return Ok(assignments);
        }

        [HttpGet("GetInProgressStages/{orderId}")]
        public async Task<IActionResult> GetInProgressStagesAsync(int orderId)
        {
            if (!await orderService.OrderExistsAsync(orderId))
            {
                return BadRequest("Order not found");
            }
            var stages = await assignmentService.GetInProgressStagesAsync(orderId);
            return Ok(stages);
        }

        [HttpGet("GetInProgressStageIds/{orderId}")]
        public async Task<IActionResult> GetInProgressStageIdsAsync(int orderId)
        {
            if (!await orderService.OrderExistsAsync(orderId))
            {
                return BadRequest("Order not found");
            }
            var stageIds = await assignmentService.GetInProgressStageIdsAsync(orderId);
            return Ok(stageIds);
        }
    }

}
