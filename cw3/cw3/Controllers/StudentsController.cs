using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Mvc;


namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18530;Integrated Security=True";
        public StudentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        // GET
        [HttpGet]
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
            string id = "s18530";
            var list = new List<Enrollment>();
            var listt = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText =
                    "SELECT IndexNumber, FirstName, LastName, Name, Semester, StartDate FROM Student INNER JOIN Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment INNER JOIN Studies on Enrollment.IdStudy = Studies.IdStudy WHERE Student.IndexNumber=@id;";
                
                com.Parameters.AddWithValue("id", indexNumber);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var enr = new Enrollment();
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    enr.Name = dr["Name"].ToString();
                    enr.Semester = dr["Semester"].ToString();
                    enr.StartDate = DateTime.Parse(dr["StartDate"].ToString()!);
                    list.Add(enr);
                    listt.Add(st);
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


        //[HttpGet("{indexNumber}")]
        //public IActionResult GetStudent(string indexNumber)
        //{
        //    using (SqlConnection con = new SqlConnection(ConString))
        //    using (SqlCommand com = new SqlCommand())
        //    {
        //        com.Connection = con;
        //        com.CommandText = "SELECT * FROM Student WHERE indexnumber=@index"+
        //            "INNER JOIN Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment"+
        //            "INNER JOIN Studies on Studies.IdStudy = Enreollment.";

        //        com.Parameters.AddWithValue("index", indexNumber);

        //        con.Open();
        //        var dr = com.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            var st = new Student();

        //            st.IndexNumber = dr["IndexNumber"].ToString();
        //            st.FirstName = dr["FirstName"].ToString();
        //            st.LastName = dr["LastName"].ToString();
        //            return Ok(st);
        //        }
        //    }
        //    return NotFound();
        //}

        //[HttpGet]
        //public IActionResult GetStudents2()
        //{
        //    var list = new List<Student>();
        //    
        //    using (SqlConnection con = new SqlConnection(ConString))
        //    using (SqlCommand com = new SqlCommand())
        //    {
        //        com.Connection = con;
        //        com.CommandText = "TestProc3";
        //        com.CommandType = System.Data.CommandType.StoredProcedure;
        //        con.Open();
        //        com.Parameters.AddWithValue("LastName", "Kowalski");
        //        var dr = com.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            var st = new Student();
        //            st.IndexNumber = dr["IndexNumber"].ToString();
        //            st.FirstName = dr["FirstName"].ToString();
        //            st.LastName = dr["LastName"].ToString();
        //            list.Add(st);
        //        }
        //    }
        //    return Ok(list);
        //}
        
        //[HttpGet]
        //public IActionResult GetStudents3()
        //{
        //    var list = new List<Student>();
        //    
        //    using (SqlConnection con = new SqlConnection(ConString))
        //    using (SqlCommand com = new SqlCommand())
        //    {
        //        com.Connection = con;
        //        com.CommandText = "INSERT INTO ...";
        //        
        //        con.Open();
        //        SqlTransaction transaction = con.BeginTransaction();

        //        try
        //        {
        //            int affectedRows = com.ExecuteNonQuery();

        //            com.CommandText = "UPDATE INTO ...";
        //        
        //            //...
        //            transaction.Commit();
        //        }
        //        catch (Exception e)
        //        {
        //            transaction.Rollback();
        //        }
        //        
        //        com.Parameters.AddWithValue("LastName", "Kowalski");
        //        var dr = com.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            var st = new Student();
        //            st.IndexNumber = dr["IndexNumber"].ToString();
        //            st.FirstName = dr["FirstName"].ToString();
        //            st.LastName = dr["LastName"].ToString();
        //            list.Add(st);
        //        }
        //    }
        //    return Ok(list);
        //}


//[HttpGet]
//public IActionResult GetStudents0(string orderBy)
//{
//    return Ok(_dbService.GetStudents());
//}
        
//[HttpGet("{id}")]
//public IActionResult GetStudentID(int id)
//{
//    if (id == 1)
//    {
//        return Ok("Kowalski");
//    }else if (id == 2)
//    {
//        return Ok("Pilewski");
//    }
//    return NotFound("Nie znaleziono studenta");
//}