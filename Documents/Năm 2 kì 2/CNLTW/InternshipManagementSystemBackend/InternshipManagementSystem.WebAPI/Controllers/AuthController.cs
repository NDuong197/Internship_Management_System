using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InternshipManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.MatKhau) || string.IsNullOrEmpty(loginDTO.Role))
            {
                return BadRequest("Email, password, and role are required.");
            }

            object user = null;
            switch (loginDTO.Role)
            {
                case "SinhVien":
                    user = (await _unitOfWork.SinhViens.GetAllAsync())
                        .FirstOrDefault(s => s.Email == loginDTO.Email && s.MatKhau == loginDTO.MatKhau);
                    break;
                case "DoanhNghiep":
                    user = (await _unitOfWork.DoanhNghieps.GetAllAsync())
                        .FirstOrDefault(d => d.Email == loginDTO.Email && d.MatKhau == loginDTO.MatKhau);
                    break;
                case "Admin":
                    user = (await _unitOfWork.Admins.GetAllAsync())
                        .FirstOrDefault(a => a.Email == loginDTO.Email && a.MatKhau == loginDTO.MatKhau);
                    break;
                default:
                    return BadRequest("Invalid role.");
            }

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginDTO.Email),
                new Claim(ClaimTypes.Role, loginDTO.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = loginDTO.Role
            });
        }
    }
}