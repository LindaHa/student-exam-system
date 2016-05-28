using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class SolutionQuestion
    {
        public Question Question { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Solution Solution { get; set; }
        [Required]
        public int SolutionId { get; set; }

    }
}
