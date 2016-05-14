using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{
    public class Answer
    {       
        [Required]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
