using BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class StudentGroupModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public List<StudentDTO> Students { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
        public TeacherDTO Teacher { get; set; }
    }
}