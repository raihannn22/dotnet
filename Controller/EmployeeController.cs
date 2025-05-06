using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
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

    [HttpGet("get/list")]
    public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetListEmployees()
    {
         IEnumerable<EmployeeResponse> response =  await _employeeService.GetListEmployees();
         Console.WriteLine(response.Count());
         return Ok(response);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeById(long id)
    {
        EmployeeResponse response = await _employeeService.GetEmployeeById(id);
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeByName([FromQuery]string name)
    {
        EmployeeResponse response = await _employeeService.GetEmployeeByName(name);
        return Ok(response);
    }

    [HttpPost("save-update")]
    public async Task<IActionResult> SaveOrUpdate([FromBody] EmployeeResponse employeeResponse)
    {
        EmployeeResponse response = await _employeeService.SaveOrUpdate(employeeResponse);
        return Ok(response);
    }

}