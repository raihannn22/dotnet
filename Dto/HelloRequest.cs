namespace SampleApi.Dto;

public class HelloRequest
{
    public string Name { get; set; }

    public HelloRequest()
    {
            
    }
    public HelloRequest(string messageName)
    {
        Name = messageName;
    }
}