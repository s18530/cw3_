using System;
using System.Data.SqlClient;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18530;Integrated Security=True";
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        // GET
        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM Students";
                
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    
                }
            }
            
            return Ok(_dbService.GetStudents());
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
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
        
        //PUT
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(Student student, int id)
        {
            student.IdStudent = id;
            return Ok("Aktualizacja dokończona");
        }
        
        //DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(Student student, int id)
        {
            //usuniecie
            return Ok("Usuwanie ukończone");
        }
    }
}