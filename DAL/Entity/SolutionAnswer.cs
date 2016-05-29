using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class SolutionAnswer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
        [Required]
        public int SolutionId { get; set; }
        public Solution Solution { get; set; }
    }
}
