namespace SampleApi.Dto;

public class HelloResponse
{
    public string Name { get; set; }
    
    public HelloResponse(string message)
    {
        Name = message;
    }

    HelloResponse()
    {
        
    }

   
}