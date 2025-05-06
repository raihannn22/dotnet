using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SampleApi.Data;
using SampleApi.Dto;
using SampleApi.Entity;
using SampleApi.Repositories;

namespace SampleApi.Service;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly AppDbContext _context;

    public EmployeeService(IEmployeeRepository employeeRepository, AppDbContext context)
    {
        _employeeRepository = employeeRepository;
        _context = context;
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

    public async Task<IEnumerable<EmployeeResponse>> GetListEmployees() 
    {
        IEnumerable<Employee> employees = await _employeeRepository.GetAll();
        IEnumerable<EmployeeResponse> employeesResponse = employees.Select(e => new EmployeeResponse()
        {
            Id = e.Id,
            Name = e.Name
        });
        return employeesResponse;
    }

    public async Task<EmployeeResponse> GetEmployeeById(long id)
    {
        Employee employee = await _context.Employees.FindAsync(id);
        return new EmployeeResponse()
        {
            Id = employee.Id,
            Name = employee.Name,
            
        };
    }

    public async Task<EmployeeResponse> GetEmployeeByName(string name)
    {
        Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Name == name);
        if (employee == null)
        {
            throw new Exception("Employee not found");
        }
        EmployeeResponse response = new EmployeeResponse(employee);
        return response;
    }

    public async Task<EmployeeResponse> SaveOrUpdate(EmployeeResponse employeeResponse)
    {
        Employee employee = new Employee();
        if (employeeResponse.Id != null)
        {
            employee = _context.Employees.FirstOrDefault(x => x.Id == employeeResponse.Id);
            if (employee == null)
            {
                throw new Exception("Employee not found");
            }
            employee.Name = employeeResponse.Name;
        }
        else
        {
            employee.Name = employeeResponse.Name;
            await _context.Employees.AddAsync(employee);
        }
        
        await _context.SaveChangesAsync();
        return new EmployeeResponse()
        {
            Id = employee.Id,
            Name = employee.Name
        };
    }


}