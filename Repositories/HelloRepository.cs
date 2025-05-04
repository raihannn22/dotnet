namespace SampleApi.Repositories;

public class HelloRepository : IHelloRepository
{
    public string GetHello()
    {
        return "coba haii";
    }
}