using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class TestPatternDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfQuestions { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public AreaDTO Area { get; set; }
        public StudentGroupDTO StudentGroup { get; set; }
        [Required]
        public TeacherDTO Teacher { get; set; }
    }
}
