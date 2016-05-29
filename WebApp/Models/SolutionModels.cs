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
    public class SolutionAnswerModel
    {
        public int Id { get; set; }
        public bool isSelected { get; set; }
        public string Text { get; set; }
    }

    public class SolutionQuestionModel
    {
        public SolutionQuestionModel()
        {
            Answers = new List<SolutionAnswerModel>();
        }
        public int Id { get; set; }
        public List<SolutionAnswerModel> Answers { get; set; }
        public string Text { get; set; }
        public string Explanation { get; set; }
        public string TypeOfQuestion { get; set; }
        public int SelectedAnswerID { get; set; }
    }

    public class SolutionModel
    {
        public SolutionModel()
        {
            Questions = new List<SolutionQuestionModel>();
        }
        public int Id { get; set; }
        public List<SolutionQuestionModel> Questions { get; set; }
    }
}