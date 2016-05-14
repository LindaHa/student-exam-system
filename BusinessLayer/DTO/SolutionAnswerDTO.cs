using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class SolutionAnswerDTO
    {
        public int Id { get; set; }
        [Required]
        public AnswerDTO Answer { get; set; }
        [Required]
        public SolutionDTO Solution { get; set; }
    }
}
