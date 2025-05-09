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
        var division = _context.Divisions.Find(employeeRequest.DivisionId);
        if (division == null)
        {
            throw new Exception("Division not found");
        }
        
        var employee = new Employee
        {
            Name = employeeRequest.Name,
            Email = employeeRequest.Email,
            Salary = employeeRequest.Salary,
            Address = employeeRequest.Address,
            DivisionId = employeeRequest.DivisionId
        };

        var saved = await _employeeRepository.Save(employee);

        return new EmployeeResponse(employee);
    }

    public async Task<EmployeeResponse> updateEmployee(EmployeeRequest employeeRequest , long id)
    {
        Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        employee.Name = employeeRequest.Name;
        employee.Email = employeeRequest.Email;
        employee.Salary = employeeRequest.Salary;
        employee.Address = employeeRequest.Address;
        employee.DivisionId = employeeRequest.DivisionId;

        await _context.SaveChangesAsync();

        return new EmployeeResponse(employee);
    }

    public async Task<List<EmployeeResponse>> GetListEmployees() 
    {
        List<Employee> employees = await _context.Employees.Include(e => e.Division).ToListAsync();
        List<EmployeeResponse> employeesResponse = employees.Select(e => new EmployeeResponse(e)).ToList();
        return employeesResponse;
    }

    public async Task<EmployeeResponse> GetEmployeeById(long id)
    {
        Employee employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new Exception("Employee not found");
        }
        return new EmployeeResponse(employee);
    }

    public async Task<List<EmployeeResponse>> GetEmployeeByName(string? name, string? email, long? maxSalary, long? minSalary, string? address, 
        long? divisionId,
        string? divisionName,
        bool isAscending = true,  
        int pageNumber = 1 , 
        int pageSize = 20)
    {
        var employees = _context.Employees.Include(e => e.Division).AsQueryable();
        employees = employees.Where(x => x.IsDeleted == false);
        
        //filtering
        if (name != null)
        {
            employees = employees.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
        }
        
        if (email != null)
        {
            employees = employees.Where(x => x.Email.ToLower().Contains(email.ToLower().Trim()));
        }
        
        if (maxSalary != null)
        {
            employees = employees.Where(x => x.Salary <= maxSalary);
        }
        
        if (minSalary != null)
        {
            employees = employees.Where(x => x.Salary >= minSalary);
        }
        
        if (address != null)
        {
            employees = employees.Where(x => x.Address.ToLower().Contains(address.ToLower().Trim()));
        }
        
        if (divisionId != null)
        {
            employees = employees.Where(x => x.DivisionId == divisionId);
        }
        
        if (divisionName != null)
        {
            employees = employees.Where(x => x.Division.Name.ToLower().Contains(divisionName.ToLower().Trim()));
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
        var division = _context.Divisions.Find(employeeSaveUpdate.DivisionId);
        if (division == null)
        {
            throw new Exception("Division not found");
        }
        
        Employee employee = new Employee();
        if (employeeSaveUpdate.Id != null) //update
        {
            employee = _context.Employees.FirstOrDefault(x => x.Id == employeeSaveUpdate.Id);
            if (employee == null)
            {
                throw new Exception("Employee not found");
            }

            employee.Name = employeeSaveUpdate.Name;
            employee.Email = employeeSaveUpdate.Email;
            employee.Salary = employeeSaveUpdate.Salary;
            employee.Address = employeeSaveUpdate.Address;
            employee.DivisionId = employeeSaveUpdate.DivisionId;
        }
        else //save
        {
            employee.Name = employeeSaveUpdate.Name;
            employee.Email = employeeSaveUpdate.Email;
            employee.Salary = employeeSaveUpdate.Salary;
            employee.Address = employeeSaveUpdate.Address;
            employee.DivisionId = employeeSaveUpdate.DivisionId;
            await _context.Employees.AddAsync(employee);
        }
        
        await _context.SaveChangesAsync();
        
        return new EmployeeResponse(employee);
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