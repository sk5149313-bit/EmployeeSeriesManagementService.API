using EmployeeSeriesManagementService.Api.Contracts;
using EmployeeSeriesManagementService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSeriesManagementService.Api.Controllers;

[ApiController]
[Route("api/employees/{employeeId:int}/series")]
public class EmployeeSeriesController(ISeriesService series) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetForPeriod(
        int employeeId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        try
        {
            if (endDate < startDate)
            {
                return BadRequest(new { message = "End date cannot be less then start date " });
            }

            var result = await series.GetForPeriodAsync(employeeId, startDate, endDate);
            return result is null ? NotFound(new { message = "EmployeeId not found." }) : Ok(result);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        int employeeId, [FromBody] CreateEmployeeSeriesRequest request)
    {
        try
        {
            var result = await series.CreateForEmployeeAsync(employeeId, request);
            if (result is null) return NotFound(new { message = "EmployeeId not found." });

            return CreatedAtAction(nameof(GetForPeriod),
                new { employeeId, startDate = result.AssignmentStartDate,
                    endDate = result.AssignmentEndDate ?? result.AssignmentStartDate },
                result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
