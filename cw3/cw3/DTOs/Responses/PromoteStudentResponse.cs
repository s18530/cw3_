﻿using System;

namespace cw3.DTOs.Responses
{
    public class PromoteStudentResponse
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }
    }
}