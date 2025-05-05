using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Dto;
using SampleApi.Service;

namespace SampleApi.Controller;

[Microsoft.AspNetCore.Components.Route("employee/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] EmployeeRequest employeeRequest)
    {
        if (employeeRequest == null)
        {
            return BadRequest("gaa boleh kosong!!");
        }

        var employeeResponse = await _employeeService.SaveEmployee(employeeRequest);

        return Ok(employeeResponse);
    }
}