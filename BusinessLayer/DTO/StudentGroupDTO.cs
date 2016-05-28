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
            Students = new List<StudentDTO>();
            TestPatterns = new List<TestPatternDTO>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public List<StudentDTO> Students { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
        public TeacherDTO Teacher { get; set; }
    }
}
