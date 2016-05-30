using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class StudentGroupDTO
    {
        public StudentGroupDTO()
        {
            Enrollments = new List<EnrollmentDTO>();
            TestPatterns = new List<TestPatternDTO>();
        }
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public List<EnrollmentDTO> Enrollments { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
        public TeacherDTO Teacher { get; set; }
    }
}
