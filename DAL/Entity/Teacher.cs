using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entity
{   //Vyučující má administrační rozhraní, které mu umožňuje spravovat tematické okruhy, 
    //otázky, šablony testů, skupiny studentů a seznam registrovaných studentů. 
    //Podstatné je, že vyučující může efektivně včetně grafického zobrazení procházet 
    //výsledky testů svých studentů.
    public class Teacher
    {
        public Teacher()
        {
            TestPatterns = new List<TestPattern>();
            StudentGroups = new List<StudentGroup>();
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        public List<TestPattern> TestPatterns { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }

    }
}
