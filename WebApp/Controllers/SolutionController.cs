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
            SolutionModel solutionModel = new SolutionModel();
            solutionModel.Points = solution.Points;
            solutionModel.Start = solution.Start;
            solutionModel.StudentName = solution.Student.Surname + ", " + solution.Student.FirstName;
            foreach(var question in solution.SolutionQuestions)
            {
                SolutionQuestionModel sqm = new SolutionQuestionModel();
                sqm.Explanation = question.Question.Explanation;
                sqm.Text = question.Question.Text;
                sqm.TypeOfQuestion = question.Question.TypeOfQuestion;
                foreach(var answer in question.Question.Answers)
                {
                    SolutionAnswerModel sam = new SolutionAnswerModel();
                    bool found = false;
                    foreach(var solutionAnswer in solution.SolutionAnswers)
                    {
                        if (solutionAnswer.Answer.Id == answer.Id)
                        {
                            found = true;
                            break;
                        }
                    }
                    sam.isSelected = found;
                    sam.Text = answer.Text;
                    sam.isCorrect = answer.IsCorrect;             
                }
            }
            
            return View(solution);
        }

        public ActionResult Create(int id)
        {
            UserFacade userFacade = new UserFacade();
            var user = userFacade.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));
            TestPatternFacade patternFacade = new TestPatternFacade();
            List<QuestionDTO> generatedQuestions = patternFacade.GetTestQuestions(id);
            TestPatternDTO pattern = patternFacade.GetTestPatternById(id);
            DateTime end;
            if (DateTime.Now.AddMinutes(pattern.Time).CompareTo(pattern.End) < 0)
                end = DateTime.Now.AddMinutes(pattern.Time);
            else
                end = pattern.End;

            var newSolution = new SolutionDTO();
            newSolution.End = end;
            newSolution.Start = DateTime.Now;
            newSolution.Student = user.Student;
            newSolution.Points = 0;
            newSolution.TestPattern = pattern;

            newSolution = solutionFacade.CreateSolution(newSolution);

            SolutionModel solutionModel = new SolutionModel();
            solutionModel.SolutionId = newSolution.Id;
            solutionModel.End = end;

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
            SolutionDTO mySolution = solutionFacade.GetSolutionById(solution.SolutionId);
            //mySolution.Points = solutionFacade.GetPoints(solution);
            List<SolutionAnswerDTO> myAnswers = new List<SolutionAnswerDTO>();
            if (mySolution.End.CompareTo(DateTime.Now) < 0)
            {
                TempData["SolutionTextWarning"] = "Your time is up. You shall not pass.";                
                return RedirectToAction("Index");
            }                
            mySolution.End = DateTime.Now;
            mySolution.IsDone = true;
            AnswerFacade answerFacade = new AnswerFacade();
            foreach (SolutionQuestionModel question in solution.Questions)
            {
                string typeOfQuestion = question.TypeOfQuestion;
                if (typeOfQuestion.Equals("single"))
                {
                    SolutionAnswerDTO myAnswer = new SolutionAnswerDTO();
                    myAnswer.Answer = answerFacade.GetAnswerById(question.SelectedAnswerId);
                    myAnswer.Solution = mySolution;
                    myAnswers.Add(myAnswer);
                }
                else if (typeOfQuestion.Equals("multiple"))
                {
                    foreach(SolutionAnswerModel answer in question.Answers)
                    {
                        if(answer.isSelected)
                        {
                            SolutionAnswerDTO myAnswer = new SolutionAnswerDTO();
                            myAnswer.Answer = answerFacade.GetAnswerById(answer.Id);
                            myAnswer.Solution = mySolution;
                            myAnswers.Add(myAnswer);
                        }
                    }
                }
            }
            mySolution.SolutionAnswers = myAnswers;
            mySolution.Points = solutionFacade.GetPoints(mySolution);
            solutionFacade.ModifySolution(mySolution);
            return RedirectToAction("Show","Solution");
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