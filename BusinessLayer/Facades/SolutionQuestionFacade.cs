using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DTO;
using DAL;
using DAL.Entity;
using System.Data.Entity;

namespace BusinessLayer.Facades
{
    public class SolutionQuestionFacade
    {
        public void CreateSolutionQuestion(SolutionQuestionDTO solutionQuestion)
        {
            SolutionQuestion newQuestion = Mapping.Mapper.Map<SolutionQuestion>(solutionQuestion);
            if (newQuestion.Solution.Id > 0)
            {
                newQuestion.Solution = null;
                newQuestion.SolutionId = solutionQuestion.Solution.Id;
            }
            if (newQuestion.Question.Id > 0)
            {
                newQuestion.Question = null;
                newQuestion.QuestionId = solutionQuestion.Question.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.SolutionQuestion.Add(newQuestion);
                context.SaveChanges();
            }
        }  
    }
}
