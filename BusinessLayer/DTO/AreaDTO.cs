using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTO
{
    public class AreaDTO
    {
        public AreaDTO()
        {
            Questions = new List<QuestionDTO>();
            TestPatterns = new List<TestPatternDTO>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Area name")]
        public string Name { get; set; }
        public List<QuestionDTO> Questions { get; set; }
        public List<TestPatternDTO> TestPatterns { get; set; }
    }
}
