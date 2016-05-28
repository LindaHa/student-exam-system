using BL.Facades;
using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Student")]
    public class AolutionController : Controller
    {
        public SolutionFacade solutionFacade = new SolutionFacade();

        public ActionResult Show(int id)
        {
            SolutionDTO solution = solutionFacade.GetSolutionById(id);
            return View(solution);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["SolutionTextWarning"];
            var model = solutionFacade.GetAllSolutions();
            return View(model);
        }

        public ActionResult Create(int testPatternId)
        {
            UserFacade userFacade = new UserFacade();
            var user = userFacade.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));
            TestPatternFacade patternFacade = new TestPatternFacade();
            List<QuestionDTO> generatedQuestions = patternFacade.GetTestQuestions(testPatternId);
            TestPatternDTO pattern = patternFacade.GetTestPatternById(testPatternId);
            DateTime end;
            if (DateTime.Now.AddMinutes(pattern.Time) <= pattern.End)
                end = DateTime.Now.AddMinutes(pattern.Time);
            else
                end = pattern.End;

            var newSolution = new SolutionDTO();
            newSolution.End = end;
            newSolution.Student = user.Student;
            newSolution.IsDone = false;
            newSolution.Points = 0;
            newSolution.Start = DateTime.Now;
            newSolution.TestPattern = pattern;

            newSolution = solutionFacade.CreateSolution(newSolution);

            foreach (QuestionDTO question in generatedQuestions)
            {
                SolutionQuestionDTO solutionQuestion = new SolutionQuestionDTO();
                solutionQuestion.Question = question;
                solutionQuestion.Solution = newSolution;
                newSolution.SolutionQuestions = generatedQuestions;
            }
            
            return View(solution);
        }

        [HttpPost]
        public ActionResult Create(SolutionDTO solution)
        {            
            if (ModelState.IsValid)
            {
                TestPatternFacade testPatternFacade = new TestPatternFacade();

                

                solutionFacade.CreateSolution(newSolution);
                return RedirectToAction("Index");
            }
            return View(solution);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                solutionFacade.DeleteSolution(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("SolutionTextWarning", ex.Message);
            }
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            UserFacade userFacade = new UserFacade();
            UserDTO user = userFacade.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));
            SolutionDTO solution = solutionFacade.GetSolutionById(id);
            SolutionDTO newSolution = new SolutionDTO();

            newSolution.End = solution.End;
            newSolution.Student = user.Student;
            newSolution.Id = solution.Id;
            newSolution.IsDone = solution.IsDone;
            newSolution.Points = solution.Points;
            newSolution.SolutionAnswers = solution.SolutionAnswers;
            newSolution.SolutionQuestions = solution.SolutionQuestions;
            newSolution.Start = solution.Start;
            newSolution.TestPattern = solution.TestPattern;

            return View(newSolution);
        }

        [HttpPost]
        public void Edit(int id, SolutionDTO solution)
        {
            SolutionFacade solutionFacade = new SolutionFacade();
            solutionFacade.ModifySolution(solution);
            RedirectToAction("Index");
        }
    }
}