using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Kullanıcı doğrulama (gerçek bir sistemle değiştirin)
            if (IsValidUser(model))
            {
                var token = GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid username or password." });
        }

        /// <summary>
        /// Kullanıcı kimlik doğrulama
        /// </summary>
        private bool IsValidUser(LoginModel model)
        {
            return true;
            // Mock kullanıcı doğrulama
            return model.Username == "test" && model.Password == "password";
        }

        /// <summary>
        /// JWT Token oluşturur
        /// </summary>
        private string GenerateToken(string username)
        {
            // Güvenlik anahtarını al
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Kullanıcı bilgilerini temsil eden claimler
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username)
            };

            // JWT oluşturma
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    /// <summary>
    /// Kullanıcı giriş modeli
    /// </summary>
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
