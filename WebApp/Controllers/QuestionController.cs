using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles ="Admin, Teacher")]
    public class QuestionController : Controller
    {
        public QuestionFacade questionFacade = new QuestionFacade();

        public ActionResult Show(int id)
        {
            var question = questionFacade.GetQuestionById(id);
            return View(question);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["QuestionTextWarning"];
            var model = questionFacade.GetAllQuestions();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new QuestionModel());
        }

        [HttpPost]
        public ActionResult Create(QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();

                var newQuestion = new QuestionDTO();
                newQuestion.Text = question.Text;
                newQuestion.PointsForCorrectAnswer = question.PointsForCorrectAnswer;
                newQuestion.TypeOfQuestion = question.TypeOfQuestion;
                newQuestion.Area = areaFacade.GetAreaById(question.SelectedAreaId);
                newQuestion.Explanation = question.Explanation;

                questionFacade.CreateQuestion(newQuestion);
                return RedirectToAction("Index");
            }
            return View(question);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                questionFacade.DeleteQuestion(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("QuestionTextWarning", ex.Message);
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult Edit(int id)
        {
            var question = questionFacade.GetQuestionById(id);
            QuestionModel newQuestion = new QuestionModel();
            newQuestion.Explanation = question.Explanation;
            newQuestion.Id = question.Id;
            newQuestion.PointsForCorrectAnswer = question.PointsForCorrectAnswer;
            newQuestion.SelectedAreaId = question.Area.Id;
            newQuestion.Text = question.Text;

            return View(newQuestion);
        }

        [HttpPost]
        public ActionResult Edit(int id, QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();
                QuestionFacade questionFacade = new QuestionFacade();

                var originalQuestion = questionFacade.GetQuestionById(id);
                originalQuestion.Text = question.Text;
                originalQuestion.PointsForCorrectAnswer = question.PointsForCorrectAnswer;
                originalQuestion.TypeOfQuestion = question.TypeOfQuestion;
                originalQuestion.Area = areaFacade.GetAreaById(question.SelectedAreaId);
                originalQuestion.Explanation = question.Explanation;

                questionFacade.ModifyQuestion(originalQuestion);

                return RedirectToAction("Show", new { id = originalQuestion.Id });
            }
            return View(question);
        }
    }
}