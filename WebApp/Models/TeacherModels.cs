using BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TeacherModels
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
        [Required]
        public string Password { get; set; }
    }
}