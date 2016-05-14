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
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Surname")]
        public string Surname { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
        [Required(ErrorMessage = "Please enter your Password")]
        public string Password { get; set; }
    }
}
