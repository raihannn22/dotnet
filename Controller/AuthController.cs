using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Dto;
using SampleApi.Service;
using LoginRequest = SampleApi.Dto.LoginRequest;

namespace SampleApi.Controller;

[Microsoft.AspNetCore.Components.Route("login/[controller]")]
[ApiController] 
public class AuthController : ControllerBase
{
    private readonly JWTService _jwtService;
    
    public AuthController(JWTService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("auth")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        String generate = _jwtService.generateToken(loginRequest);
        
        return Ok(new LoginResponse { Token = generate });
    }
 
}