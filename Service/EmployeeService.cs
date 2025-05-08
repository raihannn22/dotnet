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

    public async Task<EmployeeResponse> updateEmployee(EmployeeRequest employeeRequest , long id)
    {
        Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        employee.Name = employeeRequest.Name;

        _context.SaveChangesAsync();

        return new EmployeeResponse(employee);
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

    public async Task<List<EmployeeResponse>> GetEmployeeByName(string? name, bool isAscending = true, 
        int pageNumber = 1 , int pageSize = 20)
    {
        var employees = _context.Employees.AsQueryable();
        employees = employees.Where(x => x.IsDeleted == false);
        
        //filtering
        if (name != null)
        {
            employees = employees.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
        }
        if (!employees.Any())
        {
            throw new Exception("Employee not found");
        }

        //sorting
        employees = isAscending? employees.OrderBy(x => x.Id) : employees.OrderByDescending(x => x.Id);

        //paging
        var skipResult = (pageNumber - 1) * pageSize;

        employees = employees.Skip(skipResult).Take(pageSize);
    
        return employees.Select(e => new EmployeeResponse(e)).ToList();
    }

    public async Task<EmployeeResponse> SaveOrUpdate(EmployeeSaveUpdate employeeSaveUpdate)
    {
        Employee employee = new Employee();
        if (employeeSaveUpdate.Id != null)
        {
            employee = _context.Employees.FirstOrDefault(x => x.Id == employeeSaveUpdate.Id);
            if (employee == null)
            {
                throw new Exception("Employee not found");
            }
            employee.Name = employeeSaveUpdate.Name;
        }
        else
        {
            employee.Name = employeeSaveUpdate.Name;
            await _context.Employees.AddAsync(employee);
        }
        
        await _context.SaveChangesAsync();
        
        return new EmployeeResponse()
        {
            Id = employee.Id,
            Name = employee.Name
        };
    }

    public async Task<string> SoftDelete(long id)
    {
        Employee employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            employee.IsDeleted = true;
            await _context.SaveChangesAsync();
            return "data dengan id " + id + " berhasil di hapus (soft)!";
        }
        return "data dengan id " + id + " tidak ditemukan!!";
        
    }

    public async Task<string> HardDelete(long id)
    {
        Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (employee != null)
        {
            _context.Employees.Remove(employee);
            return "data dengan id " + id + " berhasil di hapus (Hard)!";
        }
        return "data dengan id " + id + " tidak ditemukan!!";
    }


}