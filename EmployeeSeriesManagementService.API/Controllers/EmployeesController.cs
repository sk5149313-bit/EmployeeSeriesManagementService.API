using EmployeeSeriesManagementService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSeriesManagementService.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController(IEmployeeService employees) : ControllerBase
{
    [HttpGet("{employeeId:int}/addresses")]
    public async Task<IActionResult> GetAddresses(int employeeId)
    {
        return Ok(await employees.GetAddressesAsync(employeeId));
    }

    [HttpGet("personaladdresses")]
    public async Task<IActionResult> GetPersonalAddresses([FromQuery] string workCity= "Brussels")
    {
        if (string.IsNullOrWhiteSpace(workCity))
        {
            return BadRequest(new { message = "workCity is required." });
        }
        return Ok(await employees.GetPersonalAddressesByWorkCityAsync(workCity));
    }

    [HttpGet("work-address")]
    public async Task<IActionResult> GetWorkCityWisepersonaladdresses([FromQuery] string workCity)
    {
        if (string.IsNullOrWhiteSpace(workCity))
        {
            return BadRequest(new { message = "workCity is required." });
        }
        return Ok(await employees.GetPersonalAddressesByWorkCityAsync(workCity));
    }

    [HttpGet("WorkCity")]
    public async Task<IActionResult> GetAllWorkCity()
    {
       
        return Ok(await employees.GetAllWorkCityAsync());
    }
}
