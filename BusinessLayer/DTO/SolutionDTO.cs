using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class SolutionDTO
    {
        public SolutionDTO()
        {
            SolutionAnswers = new List<SolutionAnswerDTO>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a Student")]
        public StudentDTO Student { get; set; }
        public List<SolutionAnswerDTO> SolutionAnswers { get; set; }
        [Required(ErrorMessage = "Please enter a Test Pattern")]
        public TestPatternDTO TestPattern { get; set; }
        public int Points { get; set; }
        [Required]
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Required]
        public bool IsDone { get; set; }
    }
}
