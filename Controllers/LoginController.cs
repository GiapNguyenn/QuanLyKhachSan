using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly QLKSDbContextcs _context;
        private readonly IConfiguration _configuration;

        public LoginController(QLKSDbContextcs context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var Login = _context.logins
                .FirstOrDefault(u => u.Username == request.UserName && u.Password == request.Password);

            if (Login == null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng");
            }

            var token = GenerateJwtToken(Login);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(Login user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("UserId", user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration["Jwt:ValidIssuer"],
                Audience = _configuration["Jwt:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
            {
                // Kiểm tra username đã tồn tại
                var existingUser = await _context.logins.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                {
                    return BadRequest("Tài khoản đã tồn tại.");
                }

                // Tạo tài khoản mới
                var user = new Login
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = request.Password 
                };

                _context.logins.Add(user);
                await _context.SaveChangesAsync();

                return Ok("Đăng ký thành công.");
            }

        // Lấy tất cả người dùng
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var nguoiDungs = await _context.logins
                .Select(nd => new LoginDto
                {
                    UserID = nd.UserID,
                    UserName = nd.Username,
                    Email = nd.Email
                })
                .ToListAsync();

            return Ok(nguoiDungs);
        }
    }
}