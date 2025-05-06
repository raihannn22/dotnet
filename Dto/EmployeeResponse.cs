using SampleApi.Entity;

namespace SampleApi.Dto;

public class EmployeeResponse
{
    public EmployeeResponse(Employee employee)
    {
        Id = employee.Id;
        Name = employee.Name;
    }
    public long? Id { get; set; }
    public string Name { get; set; }

    public EmployeeResponse()
    {
        
    }
    
}