using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entity
{
    public class Area
    {
        public Area()
        {
            Questions = new List<Question>();
            TestPatterns = new List<TestPattern>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public List<TestPattern> TestPatterns { get; set; }
    }
}
