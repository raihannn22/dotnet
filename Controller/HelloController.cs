using Microsoft.AspNetCore.Mvc;
using SampleApi.Dto;
using SampleApi.Service;

namespace SampleApi.Controller;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    private readonly IHelloService _helloService;

    public HelloController(IHelloService helloService)
    {
        _helloService = helloService;
    }

    [HttpGet]
    public string GetHello()
    {
        return _helloService.Hello();
    }

    [HttpPost]
    public HelloResponse Dto([FromBody] HelloRequest request)
    {
        var message = $"haii {request.Name}";

        return new HelloResponse(message);
    }
}