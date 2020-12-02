using cw3.DAL;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    
    public class EnrollmentsController : ControllerBase
    {
        private IDbService _service;

        public EnrollmentsController(IDbService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.FirstName = request.FirstName;
            
            var response = new EnrollStudentResponse();
            response.LastName = st.LastName;
            return Ok();
        }
    }
}