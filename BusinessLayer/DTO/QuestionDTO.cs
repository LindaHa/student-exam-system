using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class QuestionDTO
    {
        public QuestionDTO()
        {
            Answers = new List<AnswerDTO>();
        }
        public int Id { get; set; }
        public List<AnswerDTO> Answers { get; set; }
        [Required]
        public string Text { get; set; }
        public int PointsForCorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public string TypeOfQuestion { get; set; }
        [Required]
        public AreaDTO Area { get; set; }
    }
}
