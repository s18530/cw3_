using System.ComponentModel.DataAnnotations;

namespace cw3.DTOs.Requests
{
    public class PromoteStudentRequest
    {
        [Required]
        public string Name { get; set; }
        [Required] 
        public int Semester { get; set; }
    }
}