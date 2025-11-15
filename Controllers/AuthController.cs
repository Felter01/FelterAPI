using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using FelterAPI.Data;
using FelterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FelterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FelterContext _ctx;
    private readonly IConfiguration _config;

    public AuthController(FelterContext ctx, IConfiguration config)
    {
        _ctx = ctx;
        _config = config;
    }

    /// <summary>
    /// Cria (se ainda não existir) o usuário master da Felter.
    /// </summary>
    [HttpPost("register-master")]
    public async Task<IActionResult> RegisterMaster()
    {
        const string masterEmail = "lfeltertechnology@gmail.com";
        const string masterPassword = "Egdc1958";

        var existing = await _ctx.GlobalUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == masterEmail);

        if (existing != null)
        {
            return Ok(new { message = "Master já existe", masterEmail });
        }

        var user = new GlobalUser
        {
            Id = Guid.NewGuid(),
            Name = "Felter Technology Master",
            Email = masterEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(masterPassword),
            IsSuperAdmin = true,
            CreatedAt = DateTime.UtcNow
        };

        _ctx.GlobalUsers.Add(user);
        await _ctx.SaveChangesAsync();

        return Ok(new { message = "Master criado com sucesso", masterEmail });
    }

    public record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _ctx.GlobalUsers.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Usuário não encontrado" });
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Senha inválida" });
        }

        var token = GenerateJwtToken(user);

        return Ok(new
        {
            token,
            user = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.IsSuperAdmin
            }
        });
    }

    private string GenerateJwtToken(GlobalUser user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("name", user.Name),
            new("is_superadmin", user.IsSuperAdmin ? "true" : "false")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
