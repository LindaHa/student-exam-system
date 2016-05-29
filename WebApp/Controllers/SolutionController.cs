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
    [Authorize(Roles = "Student, Admin")]
    public class SolutionController : Controller
    {
        public SolutionFacade solutionFacade = new SolutionFacade();

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["SolutionTextWarning"];
            TestPatternFacade tpf = new TestPatternFacade();
            List<TestPatternDTO> testPatterns = new List<TestPatternDTO>();

            UserFacade uf = new UserFacade();
            var user = uf.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));

            StudentFacade sf = new StudentFacade();
            List<StudentGroupDTO> studentGroups = sf.GetStudentStudentGroups(user.Student.Id);
            foreach(StudentGroupDTO studentGroup in studentGroups)
            {
                testPatterns.AddRange(tpf.GetTestPatternsByGroup(studentGroup.Id));
            }

            return View(testPatterns);
        }

        public ActionResult Show(int id)
        {
            SolutionDTO solution = solutionFacade.GetSolutionById(id);
            return View(solution);
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
            newSolution.Points = 0;
            newSolution.TestPattern = pattern;

            newSolution = solutionFacade.CreateSolution(newSolution);

            SolutionModel solutionModel = new SolutionModel();
            solutionModel.Id = newSolution.Id;

            foreach (QuestionDTO question in generatedQuestions)
            {
                SolutionQuestionDTO solutionQuestion = new SolutionQuestionDTO();
                solutionQuestion.Question = question;
                solutionQuestion.Solution = newSolution;
                newSolution.SolutionQuestions.Add(solutionQuestion);

                SolutionQuestionModel sqm = new SolutionQuestionModel();
                sqm.Id = question.Id;
                sqm.Text = question.Text;
                sqm.TypeOfQuestion = question.TypeOfQuestion;
                sqm.Explanation = question.Explanation;
                foreach(AnswerDTO answer in question.Answers)
                {
                    SolutionAnswerModel sam = new SolutionAnswerModel();
                    sam.Id = answer.Id;
                    sam.Text = answer.Text;
                    sqm.Answers.Add(sam);
                }
                solutionModel.Questions.Add(sqm);
            }
            
            return View(solutionModel);
        }

        [HttpPost]
        public ActionResult Create(SolutionModel solution)
        {
            SolutionDTO mySolution = solutionFacade.GetSolutionById(solution.Id);
            //mySolution.Points = solutionFacade.GetPoints(solution);
            mySolution.End = DateTime.Now;
            mySolution.IsDone = true;
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