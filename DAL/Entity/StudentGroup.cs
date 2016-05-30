﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entity
{
    //Skupina studentů seskupuje studenty, aby se jednodušeji vyučujícímu řešila 
    //oprávnění pro jednotlivé testy.Student je do skupiny přiřazen na základě 
    //znalosti registračního kódu, který mu předá vyučující. Registrace do skupiny 
    //probíhá buď při registraci studenta do systému, nebo dodatečně pro již registrované studenty.
    public class StudentGroup
    {
        public StudentGroup()
        {
            Enrollments = new List<Enrollment>();
            TestPatterns = new List<TestPattern>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<TestPattern> TestPatterns { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
