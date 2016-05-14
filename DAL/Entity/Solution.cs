using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{
    public class Solution
    {
        public Solution()
        {
            SolutionAnswers = new List<SolutionAnswer>();
        }
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }             
        public List<SolutionAnswer> SolutionAnswers { get; set; }
        [Required]
        public int TestPatternId { get; set; }
        public TestPattern TestPattern { get; set; }        
        [Required]
        public bool IsDone { get; set; }
        //[Required]
        //public int Points { get; set; }
        [Required]
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
