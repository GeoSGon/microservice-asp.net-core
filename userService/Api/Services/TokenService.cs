using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using userService.Api.DTOs;

namespace userService.Api.Services;

public class TokenService
{
    private readonly SymmetricSecurityKey SecurityKey;
    private readonly string Issuer;
    private readonly string Audience;
    private readonly string Algorithm;

    public TokenService(IConfiguration config)
    {
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]));
        Issuer = "userService";
        Audience = "productService";
        Algorithm = SecurityAlgorithms.HmacSha256;
    }

    public string GenerateToken(UserLoginDTO userLoginDTO)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = Audience,
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userLoginDTO.Username.ToString()),
                new Claim("Roles", userLoginDTO.Role.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(SecurityKey, Algorithm)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}