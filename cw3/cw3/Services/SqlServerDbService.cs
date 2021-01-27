using System;
using System.Data;
using System.Data.SqlClient;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Services
{
    public class SqlServerDbService : ControllerBase, IStudentDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18530;Integrated Security=True";


        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            using var con = new SqlConnection(ConString);
            using var com = new SqlCommand();
            using var tran = con.BeginTransaction();
            {
                con.Open();
                com.Connection = con;
                com.Transaction = tran;
                var dr = com.ExecuteReader();

                try
                {

                    com.CommandText = "SELECT IdStudy FROM Studies WHERE Name = @Name";
                    com.Parameters.AddWithValue("Name", request.Studies);

                    if (!dr.Read())
                    {
                        //dr.Close();
                        tran.Rollback();
                        return BadRequest("Studia nie istnieją!");
                    }

                    //dr.Close();
                    com.CommandText =
                        "SELECT e.IdEnrollment, e.Semester, e.IdStudy, e.StartDate FROM Enrollment e INNER JOIN Studies st ON e.IdStudy = st.idStudy WHERE e.Semester = 1 AND st.Name = @name";
                    int idEnrollment;
                    int idStudy = (int)dr["IdStudy"];
                    DateTime startDate = DateTime.Now;
                    dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        idEnrollment = 1;
                        com.CommandText =
                            "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (IdEnrollment, 1, @IdStudy, @StartDate)";
                        com.Parameters.AddWithValue("IdStudy", request.Studies);
                        com.Parameters.AddWithValue("StartDate", startDate.ToString());
                        com.ExecuteNonQuery();
                        //dr.Close();
                    }
                    else
                    {
                        idEnrollment = (int) dr["MaxId"];
                        com.CommandText =
                            "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (IdEnrollment, 1, @IdStudy, @StartDate)";
                        com.Parameters.AddWithValue("IdStudy", request.Studies);
                        com.Parameters.AddWithValue("StartDate", startDate.ToString());
                        com.ExecuteNonQuery();
                        //dr.Close();
                    }

                    var response = new EnrollStudentResponse
                    {
                        IdEnrollment = idEnrollment,
                        Semester = 1,
                        IdStudy = idStudy,
                        StartDate = startDate
                    };

                    //dr.Close();

                    com.CommandText =
                        "SELECT * FROM Student WHERE IndexNumber = @IndexNumber";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    com.ExecuteNonQuery();

                    if (dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest("Istnieje już student o podanym numerze indeksu!");
                    }
                    else
                    {
                        com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                        com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                        com.Parameters.AddWithValue("FirstName", request.FirstName);
                        com.Parameters.AddWithValue("LastName", request.LastName);
                        com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                        com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                        com.ExecuteNonQuery();
                        tran.Commit();
                    }
                    
                    return Created("Enroll response", response);
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                    return BadRequest(exc.Message);
                }
            }
        }

        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            using var con = new SqlConnection(ConString);
            using var com = new SqlCommand();
            using var cmd = new SqlCommand("PromoteStudents", con);
            using var tran = con.BeginTransaction();
            {
                con.Open();
                com.Connection = con;
                cmd.Connection = con;
                com.Transaction = tran;
                cmd.Transaction = tran;
                var dr = com.ExecuteReader();

                try
                {
                    com.CommandText =
                        "SELECT IdEnrollment, Semester, Enrollment.IdStudy, StartDate FROM Enrollment INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy WHERE Semester = @Semester AND Name = @Name";
                    com.Parameters.AddWithValue("Name", request.Name);
                    cmd.Parameters.AddWithValue("Name", request.Name);
                    com.Parameters.AddWithValue("Semester", request.Semester);
                    cmd.Parameters.AddWithValue("Semester", request.Semester);
                    
                    int idEnrollment = (int)dr["IdEnrollment"];
                    int semester = (int)dr["Semester"] + 1;
                    int idStudy = (int)dr["IdStudy"];
                    DateTime startDate = DateTime.Now;
                    
                    if (!dr.Read())
                    {
                        //dr.Close();
                        tran.Rollback();
                        return BadRequest("Brak wpisu!");
                    }
                    else
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }

                    var response = new PromoteStudentResponse
                    {
                        IdEnrollment = idEnrollment,
                        Semester = semester,
                        IdStudy = idStudy,
                        StartDate = startDate
                    };
                    return Created("Promote response", response);
                    //dr.Close();
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                    return BadRequest(exc.Message);
                }
            }
        }
        

        public Student GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            { 
                com.Connection = con;
                com.CommandText =
                    @"SELECT s.IndexNumber,
                                       s.FirstName,
                                       s.LastName,
                                       s.BirthDate,
                                       s.IdEnrollment 
                                FROM Student s
                                WHERE s.IndexNumber = @indexNumber";
            

            com.Parameters.AddWithValue("indexNumber", indexNumber);
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            
            if (dr.Read())
            {
                return new Student
                {
                    IndexNumber = dr["IndexNumber"].ToString(),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                    IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                };
            }
            }; 
            return null;
        }
    }
}