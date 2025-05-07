using SampleApi.Dto;

namespace SampleApi.Service;

public interface IEmployeeService
{
    Task<EmployeeResponse> SaveEmployee(EmployeeRequest employeeRequest);
    
    Task<EmployeeResponse> updateEmployee(EmployeeRequest employeeRequest, long id);
    Task<IEnumerable<EmployeeResponse>> GetListEmployees();
    Task<EmployeeResponse> GetEmployeeById(long id);
    
    Task<List<EmployeeResponse>> GetEmployeeByName(string name, bool isAscending, int pageNumber , int pageSize);
    
    Task<EmployeeResponse> SaveOrUpdate(EmployeeSaveUpdate employeeSaveUpdate);

    Task<string> SoftDelete(long id);
}