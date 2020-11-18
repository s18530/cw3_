using System.Collections.Generic;
using cw3.Models;

namespace cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{FirstName = "Jan", LastName = "Kowalski"},
                new Student{FirstName = "Anna", LastName = "Malewski"},
                new Student{FirstName = "Andrzej", LastName = "Andrzejewicz"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}