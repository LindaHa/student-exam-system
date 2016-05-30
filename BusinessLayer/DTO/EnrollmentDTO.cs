using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class EnrollmentDTO
    {
        [Required]
        public StudentDTO Student { get; set; }
        [Required]
        public StudentGroupDTO StudentGroup { get; set; }
    }
}
