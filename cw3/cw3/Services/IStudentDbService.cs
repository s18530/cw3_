using cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Services
{
    public interface IStudentDbService
    {
        public IActionResult EnrollStudent(EnrollStudentRequest enrollStudentRequest);
        public IActionResult PromoteStudent(PromoteStudentRequest promoteStudentRequest);
    }
}