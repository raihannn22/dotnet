using Microsoft.AspNetCore.Http.HttpResults;
using SampleApi.Dto;
using SampleApi.Entity;
using SampleApi.Repositories;

namespace SampleApi.Service;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeResponse> SaveEmployee(EmployeeRequest employeeRequest)
    {
        var employee = new Employee
        {
            Name = employeeRequest.Name
        };

        var saved = await _employeeRepository.Save(employee);

        return new EmployeeResponse()
        {
            Id = saved.Id, 
            Name = saved.Name
        };
    }

}