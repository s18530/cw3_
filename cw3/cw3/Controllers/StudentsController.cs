using System;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        // GET
        [HttpGet]
        public string GetStudent(string orderBy)
        {
            return $"Kowalski, Pilewski, Andrzejewski sortowanie={orderBy}";
        }
        
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }else if (id == 2)
            {
                return Ok("Pilewski");
            }
            return NotFound("Nie znaleziono studenta");
        }
        
        //POST
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //TODO add to database & generate index number
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
    }
}