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
    public class AnswerModel
    {
        public int Id { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Question required")]
        [Display(Name = "Question")]
        public int SelectedQuestionId { get; set; }
        public bool IsCorrect { get; set; }
        [Required(ErrorMessage = "Please enter the text")]
        public string Text { get; set; }

        private QuestionFacade questionFacade;
        public AnswerModel()
        {
            questionFacade = new QuestionFacade();
        }

        public IEnumerable<SelectListItem> QuestionItems
        {
            get
            {                
                return questionFacade.GetAllQuestions().Select(q => new SelectListItem
                {
                    Value = q.Id.ToString(),
                    Text = q.Text
                });
            }
        }

        public IEnumerable<SelectListItem> IsCorrectItems
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "Right Answer", Value = bool.TrueString});
                items.Add(new SelectListItem { Text = "Wrong Answer", Value = bool.FalseString, Selected = true });
                return items;
            }
        }
    }
}