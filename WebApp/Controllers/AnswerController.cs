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
    [Authorize(Roles = "Admin, Teacher")]
    public class AnswerController : Controller
    {
        public AnswerFacade answerFacade = new AnswerFacade();

        public ActionResult Show(int id)
        {
            var answer = answerFacade.GetAnswerById(id);
            return View(answer);
        }

        public ActionResult Index()
        {
            var model = answerFacade.GetAllAnswers();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new AnswerModel());
        }

        [HttpPost]
        public ActionResult Create(AnswerModel answer)
        {
            if (ModelState.IsValid)
            {
                QuestionFacade questionFacade = new QuestionFacade();
                AnswerFacade answerFacade = new AnswerFacade();

                var newAnswer = new AnswerDTO();
                newAnswer.Text = answer.Text;
                newAnswer.IsCorrect = answer.IsCorrect;
                newAnswer.Question = questionFacade.GetQuestionById(answer.SelectedQuestionId);

                answerFacade.CreateAnswer(newAnswer);
                newAnswer.Question.Answers.Add(newAnswer);
                return RedirectToAction("Index");
            }
            return View(answer);
        }

        public ActionResult Delete(int id)
        {
            answerFacade.DeleteAnswer(id);
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var answer = answerFacade.GetAnswerById(id);
            AnswerModel newAnswer = new AnswerModel();
            newAnswer.Id = answer.Id;
            newAnswer.IsCorrect = answer.IsCorrect;
            newAnswer.SelectedQuestionId = answer.Question.Id;
            newAnswer.Text = answer.Text;            

            return View(newAnswer);
        }

        [HttpPost]
        public ActionResult Edit(int id, AnswerModel answer)
        {
            if (ModelState.IsValid)
            {
                QuestionFacade questionFacade = new QuestionFacade();
                AnswerFacade answerFacade = new AnswerFacade();

                var originalAnswer = answerFacade.GetAnswerById(id);
                originalAnswer.Text = answer.Text;
                originalAnswer.IsCorrect = answer.IsCorrect;
                originalAnswer.Question = questionFacade.GetQuestionById(answer.SelectedQuestionId);

                answerFacade.ModifyAnswer(originalAnswer);

                return RedirectToAction("Show", new { id = originalAnswer.Id });
            }
            return View(answer);
        }
    }
}