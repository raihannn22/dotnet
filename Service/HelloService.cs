using SampleApi.Repositories;

namespace SampleApi.Service;

public class HelloService : IHelloService
{
    private readonly IHelloRepository _helloRepository;

    public HelloService(IHelloRepository helloRepository)
    {
        _helloRepository = helloRepository;
    }
    
    
    public string Hello()
    {
        return _helloRepository.GetHello();
    }
}