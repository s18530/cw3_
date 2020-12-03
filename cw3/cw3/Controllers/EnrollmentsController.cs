using System;
using System.Data.SqlClient;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);
            var response = new EnrollStudentResponse();
            return Ok(response);
        }

        [Route("api/enrollments/promotions")]
        [HttpPost]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            _service.PromoteStudent(request);
            var response = new PromoteStudentResponse();
            return Ok(response);
        }
    }
}