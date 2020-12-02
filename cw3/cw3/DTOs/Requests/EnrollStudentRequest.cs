using System;
using System.ComponentModel.DataAnnotations;

namespace cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        //[RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "Musisz podać imię")]
        [MaxLength(10)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public string Studies { get; set; }
        public DateTime Birthdate { get; set; }
    }
}