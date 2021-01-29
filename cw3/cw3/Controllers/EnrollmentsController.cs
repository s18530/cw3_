using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace cw3.Controllers
{
    
    [ApiController]
    [Route("api/enrollments")]
    
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;
        public IConfiguration Configuration { get; set; }
        
        public EnrollmentsController(IStudentDbService service, IConfiguration configuration)
        {
            _service = service;
            Configuration = configuration;
        }
        
        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);
            var response = new EnrollStudentResponse();
            return Ok(response);
        }

        
        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            _service.PromoteStudent(request);
            var response = new PromoteStudentResponse();
            return Ok(response);
        }
        
        //LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto loginRequestDto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Konrad123"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student"),
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );
            
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}