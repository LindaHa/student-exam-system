using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TestPatternModel
    {        
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the number of questions")]
        public int NumberOfQuestions { get; set; }
        [Required(ErrorMessage = "Please enter a starting time")]       
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "Please enter the ending time")]
        public DateTime End { get; set; }
        [Required(ErrorMessage = "Please enter a time-span")]
        public int Time { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Area required")]
        [Display(Name = "Area")]
        public int SelectedAreaId { get; set; }
        [Display(Name = "StudentGroup")]
        public int SelectedStudentGroupId { get; set; }
        public TeacherDTO Teacher { get; set; }

        private AreaFacade areaFacade;
        public TestPatternModel()
        {
            areaFacade = new AreaFacade();
        }

        public IEnumerable<SelectListItem> AreaItems
        {
            get
            {
                return areaFacade.GetAllArea().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                });
            }
        }        

        public IEnumerable<SelectListItem> StudentGroupItems
        {
            get
            {
                StudentGroupFacade studentGroupFacade = new StudentGroupFacade();
                return studentGroupFacade.GetAllStudentGroups().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Id.ToString()
                });
            }
        }
    }
}