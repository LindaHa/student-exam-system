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
    public class QuestionModel
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Area required")]
        [Display(Name = "Area")]
        public int SelectedAreaId { get; set; }
        public int Id { get; set; }
        public List<AnswerDTO> Answers { get; set; }
        [Required(ErrorMessage = "Please enter Question text")]
        public string Text { get; set; }
        public int PointsForCorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public string TypeOfQuestion { get; set; }

        private AreaFacade areaFacade;
        public QuestionModel()
        {
            areaFacade = new AreaFacade();
        }

        public IEnumerable<SelectListItem> AreaItems
        {
            get {
                areaFacade = new AreaFacade();
                return areaFacade.GetAllArea().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                });
            }
        }

        public IEnumerable<SelectListItem> QuestionTypeItems
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Single-Choice", Value = "single", Selected = true });
                items.Add(new SelectListItem { Text = "Multiple-Choice", Value = "multiple" });
                return items;
            }
        }

    }
}