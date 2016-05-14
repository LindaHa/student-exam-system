using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entity
{   //Šablona testu popisuje základní parametry testu: název testu, čas na vypracování, 
    //která skupina studentů může v definovaném časovém intervalu přistupovat k testu, 
    //který je tvořen náhodným výběrem určeného počtu testových otázek z určených tematických okruhů
    public class TestPattern
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfQuestions { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public int AreaId { get; set; }
        public Area Area { get; set; }
        public int StudentGroupId { get; set; }
        public StudentGroup StudentGroup { get; set; }        
        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
