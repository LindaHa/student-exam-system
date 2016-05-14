    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{   // typem otázky (1 správná odpověď nebo 0-n správných odpovědí), 
    // definicí odpovědí, počtem bodů za správnou odpověď a vysvětlením, 
    // které se může zobrazit při prohlížení vyhodnocení testu.
    public class Question
    {
        public Question()
        {           
            Answers = new List<Answer>();
        }
        [Required]
        public int Id { get; set; }
        public List<Answer> Answers { get; set; }
        [Required]
        public string Text { get; set; }        
        [Required]
        public int PointsForCorrectAnswer { get; set; }
        public string Explanation { get; set; }
        [Required]
        public string TypeOfQuestion { get; set; }
        [Required]
        public int AreaId { get; set; }
        public Area Area { get; set; }

    }
}
