using SampleApi.Entity;

namespace SampleApi.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> Save(Employee employee);
    Task<IEnumerable<Employee>> GetAll();
}