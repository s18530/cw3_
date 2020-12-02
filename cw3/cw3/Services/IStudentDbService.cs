using System.Collections.Generic;
using cw3.DTOs.Requests;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.DAL
{
    public interface IStudentDbService
    {
        public IActionResult EnrollStudent(EnrollStudentRequest enrollStudentRequest);
    }
}