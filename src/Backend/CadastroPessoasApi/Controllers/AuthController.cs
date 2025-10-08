using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CadastroPessoasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        
        private static readonly List<(string Username, string Password)> _users = new()
        {
            ("admin", "password123"),
            ("user", "password123")
        };

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    [HttpPost("login")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!_users.Any(u => u.Username == request.Username && u.Password == request.Password))
                return Unauthorized(new { message = "Credenciais inválidas" });

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings.GetValue<string>("Secret") ?? "very_secret_key_change_me";
            var issuer = jwtSettings.GetValue<string>("Issuer") ?? "myapi";

            var claims = new[] { new Claim(ClaimTypes.Name, request.Username) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        public record LoginRequest(string Username, string Password);
    }
}
