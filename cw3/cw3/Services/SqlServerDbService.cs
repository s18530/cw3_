using System;
using System.Data.SqlClient;
using cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using cw3.DTOs.Responses;


namespace cw3.DAL
{
    public class SqlServerStudentDbService : ControllerBase, IStudentDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18530;Integrated Security=True";


        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18530;Integrated Security=True"))
            using (var com = new SqlCommand())
            using (var tran = con.BeginTransaction())
            {
                con.Open();
                com.Connection = con;
                com.Transaction = tran;
                var dr = com.ExecuteReader();

                try
                {

                    com.CommandText = $"SELECT IdStudy FROM Studies WHERE Name = @name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Nie znaleziono studiów o podanej nazwie");
                    }

                    dr.Close();
                    com.CommandText =
                        $"SELECT e.IdEnrollment, e.Semester, e.IdStudy FROM Enrollment e INNER JOIN Studies st ON e.IdStudy = st.idStudyWHERE e.Semester = 1 AND st.Name = @name";
                    int idStudy = (int) dr["IdStudy"];
                    int idEnrollment;
                    dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        idEnrollment = 1;
                        com.CommandText =
                            $"INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (IdEnrollment, 1, @IdStudy, @StartDate)";
                        dr.Close();

                    }
                    else
                    {
                        idEnrollment = (int) dr["MaxId"];
                        com.CommandText =
                            $"INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (IdEnrollment, 1, @IdStudy, @StartDate)";
                        dr.Close();

                    }

                    com.Parameters.AddWithValue("IdStudy", request.Studies);
                    com.Parameters.AddWithValue("StartDate", DateTime.Now.ToString());
                    com.ExecuteNonQuery();

                    var response = new EnrollStudentResponse
                    {
                        IdEnrollment = idEnrollment,
                        Semester = 1,
                        Name = dr["Name"].ToString(),
                        StartDate = DateTime.Parse(dr["StartDate"].ToString())

                    };

                    dr.Close();

                    com.CommandText =
                        $"INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", request.FirstName);
                    com.Parameters.AddWithValue("LastName", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    com.Parameters.AddWithValue("IdEnrollment", idEnrollment);

                    com.ExecuteNonQuery();
                    com.Transaction.Commit();
                    return Ok(response);

                }
                catch (SqlException exc)
                {
                    com.Transaction.Rollback();
                    return BadRequest(exc.Message);
                }
            }
        }
    }
}