using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Dto;
using SampleApi.ExceptionHandler;
using SampleApi.Service;

namespace SampleApi.Controller;

[Microsoft.AspNetCore.Components.Route("employee/[controller]")]
[ApiController]
[Authorize] 
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

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromBody] EmployeeRequest employeeRequest, long id)
    {
        if (employeeRequest == null)
        {
            return BadRequest("gaa boleh kosong!!");
        }

        var employeeResponse = await _employeeService.updateEmployee(employeeRequest, id);

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

    [HttpGet("getByName")]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployeeByName([FromQuery]string? name, [FromQuery]bool? isAscending,
        [FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 20)
    {
        var response = await _employeeService.GetEmployeeByName(name, isAscending ?? true, pageNumber, pageSize);
        return Ok(response);
    }

    [HttpPost("save-update")]
    [ValidateModel]
    public async Task<IActionResult> SaveOrUpdate([FromBody] EmployeeSaveUpdate employeeSaveUpdate)
    {
            EmployeeResponse response = await _employeeService.SaveOrUpdate(employeeSaveUpdate);
            return Ok(response); 
    }

    [HttpDelete("sofdelete/{id}")]
    public async Task<string> sofDelete(long id)
    {
        string response = await _employeeService.SoftDelete(id);
        return response;
    }

}