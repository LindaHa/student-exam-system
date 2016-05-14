using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class StudentDTO
    {
        public StudentDTO()
        {
            Solutions = new List<SolutionDTO>();
            StudentGroups = new List<StudentGroupDTO>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill out your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please fill out your Surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Please enter your Password")]
        public string Password { get; set; }

        public List<SolutionDTO> Solutions { get; set; }
        public List<StudentGroupDTO> StudentGroups { get; set; }
    }
}
