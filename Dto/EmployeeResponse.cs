using SampleApi.Entity;

namespace SampleApi.Dto;

public class EmployeeResponse
{
    public EmployeeResponse(Employee employee)
    {
        Id = employee.Id;
        Name = employee.Name;
        Email = employee.Email;
        Salary = employee.Salary;
        Address = employee.Address;
        DivisionId = employee.DivisionId;
        DivisionName = employee.Division?.Name ?? "N/a";
    }
    public long? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public long Salary { get; set; }
    public string Address { get; set; }
    public long DivisionId { get; set; }
    public string DivisionName { get; set; }

    public EmployeeResponse()
    {
        
    }
    
}