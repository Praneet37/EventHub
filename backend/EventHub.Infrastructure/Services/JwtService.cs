using EventHub.Application.Users.Interfaces;
using Microsoft.Extensions.Configuration; 
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EventHub.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration config;
    
   public JwtService(IConfiguration configuration)
    {
        config = configuration;
    }

    public string GenerateToken(Guid id, string email, string fullName)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("FullName", fullName)
        };
        var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer : config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
           expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(config["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}