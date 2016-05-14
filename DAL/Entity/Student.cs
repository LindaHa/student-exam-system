using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entity
{   // Student se registruje do systému, přičemž uvádí základní osobní údaje.
    // Po přihlášení vidí seznam testů, které má přístupné a může test otevřít a začít vyplňovat.
    public class Student
    {
        public Student()
        {
            Solutions = new List<Solution>();
            StudentGroups = new List<StudentGroup>();
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
        public List<Solution> Solutions { get; set; }



    }
}
