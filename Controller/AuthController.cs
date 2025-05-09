
using Microsoft.AspNetCore.Mvc;
using SampleApi.Dto;
using SampleApi.ExceptionHandler;
using SampleApi.Service;
using LoginRequest = SampleApi.Dto.LoginRequest;

namespace SampleApi.Controller;

[Route("[controller]")]
[ApiController] 
public class AuthController : ControllerBase
{
    private readonly JWTService _jwtService;
    
    public AuthController(JWTService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [ValidateModel]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        String generate = _jwtService.generateToken(loginRequest);
        
        return Ok(new LoginResponse { Token = generate });
    }

    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] LoginRequest loginRequest)
    {
        string response = await _jwtService.Register(loginRequest);
        return Ok(response);
    }
 
}