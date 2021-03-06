﻿using System;
using System.Data;

namespace cw3.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public int IdEnrollment { get; set; }
        public int IdStudy { get; set; }
        public string LastName { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public string Name { get; set; }
    }
}