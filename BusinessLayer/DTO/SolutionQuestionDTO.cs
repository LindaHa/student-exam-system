using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class SolutionQuestionDTO
    {
        [Required]
        public QuestionDTO Question { get; set; }
        [Required]
        public SolutionDTO Solution { get; set; }
    }
}
