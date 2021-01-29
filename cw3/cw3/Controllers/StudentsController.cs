using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cw3.DTOs.Requests;
using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        public IConfiguration Configuration { get; set; }
        private readonly IStudentDbService _dbService;
        
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18530;Integrated Security=True";
        public StudentsController(IStudentDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            Configuration = configuration;
        }

        // GET
        [HttpGet]
        [Authorize]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText =
                    "SELECT FirstName, LastName, IndexNumber, Name, Semester FROM Student INNER JOIN Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment INNER JOIN Studies on Studies.IdStudy = Enrollment.IdStudy;";
                
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.StudiesName = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    list.Add(st);
                }
            }
            return Ok(list);
        }
        
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudents(string indexNumber)
        {
            //id = "s18530";
            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText =
                    "SELECT * FROM Student WHERE IndexNumber=@indexNumber;";
                
                com.Parameters.AddWithValue("indexNumber", indexNumber);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    st.IdEnrollment = (int)dr["IdEnrollment"];
                    list.Add(st);
                    return Ok(st);
                }
            }
            return NotFound();
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
        public IActionResult UpdateStudent(Student student, string id)
        {
            student.IndexNumber = id;
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