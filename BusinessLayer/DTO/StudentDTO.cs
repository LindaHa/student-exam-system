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
            Enrollments = new List<EnrollmentDTO>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill out your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please fill out your Surname")]
        public string Surname { get; set; }
        public List<SolutionDTO> Solutions { get; set; }
        public List<EnrollmentDTO> Enrollments { get; set; }
    }
}
