using SampleApi.Dto;

namespace SampleApi.Service;

public interface IEmployeeService
{
    Task<EmployeeResponse> SaveEmployee(EmployeeRequest employeeRequest);
    Task<IEnumerable<EmployeeResponse>> GetListEmployees();
    Task<EmployeeResponse> GetEmployeeById(long id);
    
    Task<List<EmployeeResponse>> GetEmployeeByName(string name);
    
    Task<EmployeeResponse> SaveOrUpdate(EmployeeSaveUpdate employeeSaveUpdate);
}