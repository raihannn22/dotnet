using SampleApi.Data;
using SampleApi.Dto;
using SampleApi.Entity;

namespace SampleApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> Save(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        IEnumerable<Employee> employees = new List<Employee>();
        employees = _context.Employees.ToList();
        return employees;
        
    }

    
}