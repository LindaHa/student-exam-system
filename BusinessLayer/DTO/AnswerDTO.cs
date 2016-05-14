using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public QuestionDTO Question { get; set; }      
        public bool IsCorrect { get; set; }       
        public string Text { get; set; }
    }
}
