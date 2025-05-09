using SampleApi.Dto;

namespace SampleApi.Service;

public interface IEmployeeService
{
    Task<EmployeeResponse> SaveEmployee(EmployeeRequest employeeRequest);
    
    Task<EmployeeResponse> updateEmployee(EmployeeRequest employeeRequest, long id);
    Task<List<EmployeeResponse>> GetListEmployees();
    Task<EmployeeResponse> GetEmployeeById(long id);
    
    Task<List<EmployeeResponse>> GetEmployeeByName(string? name, string? email, long? maxSalary, long? minSalary, string? address, 
        bool isAscending = true,  
        int pageNumber = 1 , 
        int pageSize = 20);
    
    Task<EmployeeResponse> SaveOrUpdate(EmployeeSaveUpdate employeeSaveUpdate);

    Task<string> SoftDelete(long id);
    
    Task<string> HardDelete(long id);
}