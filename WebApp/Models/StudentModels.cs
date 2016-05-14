using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        public List<SolutionDTO> Solutions { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "Student-Groups")]
        public List<int> SelectedStudentGroupIds { get; set; }

        private StudentGroupFacade studentGroupFacade;
        public StudentModel()
        {
            studentGroupFacade = new StudentGroupFacade();
        }
        public IEnumerable<SelectListItem>StudentGroupItems
        {
            get
            {
                studentGroupFacade = new StudentGroupFacade();
                return studentGroupFacade.GetAllStudentGroups().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Id.ToString()
                });
            }
        }
    }
}