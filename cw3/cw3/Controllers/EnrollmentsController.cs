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
    }
}