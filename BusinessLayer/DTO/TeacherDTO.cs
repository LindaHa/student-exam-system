using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class TeacherDTO
    {
        public TeacherDTO()
        {
            TestPatterns = new List<TestPatternDTO>();
            StudentGroups = new List<StudentGroupDTO>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Surname")]
        public string Surname { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
        public List<StudentGroupDTO> StudentGroups { get; set; }
    }
}
