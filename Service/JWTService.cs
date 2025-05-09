using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleApi.Data;
using SampleApi.Entity;
using LoginRequest = SampleApi.Dto.LoginRequest;

namespace SampleApi.Service;

public class JWTService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<AppUser> _hasher;
    
    public JWTService(IConfiguration configuration, AppDbContext context, IPasswordHasher<AppUser> hasher)
    {
        _configuration = configuration;
        _context = context;
        _hasher = hasher;
    }

    public string generateToken(LoginRequest loginRequest)
    {
        // Cari user berdasarkan username
        var user = _context.Users
            .FirstOrDefault(u => u.Username == loginRequest.Username);

        // Jika user tidak ditemukan → unauthorized
        if (user == null)
        {
            return "salah!!";
        }

        // Verifikasi password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return "salah!!";
        }
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, loginRequest.Username)
        };
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience : _configuration["Jwt:Audience"],
            claims : claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> Register(LoginRequest loginRequest)
    {
        var ExistUser = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == loginRequest.Username.ToLower());

        if (ExistUser != null)
        {
            return "nama pengguna sudah ada!";
        }

        AppUser user = new AppUser()
        {
            Username = loginRequest.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password),
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return "berhasil membuat user dengan username " + loginRequest.Username + ", silahkan login!";
    }
}