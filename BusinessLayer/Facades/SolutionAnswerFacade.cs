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
    class SolutionAnswerFacade
    {
        public void CreateSolutionAnswer(SolutionAnswerDTO solutionAnswer)
        {
            SolutionAnswer newAnswer = Mapping.Mapper.Map<SolutionAnswer>(solutionAnswer);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.SolutionAnswer.Add(newAnswer);
                context.SaveChanges();
            }        
        }

        public void DeleteSolutionAnswer(int solutionAnswerId)
        {
            using (var context = new AppDbContext())
            {
                var answer = context.SolutionAnswer.Find(solutionAnswerId);
                answer.Solution.SolutionAnswers.Remove(answer);
                context.Entry(answer).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public SolutionAnswerDTO GetSolutionAnswerById(int solutionAnswerId)
        {
            using (var context = new AppDbContext())
            {
                var answer = context.SolutionAnswer.Find(solutionAnswerId);
                return Mapping.Mapper.Map<SolutionAnswerDTO>(answer);
            }
        }

        public List<SolutionAnswerDTO> GetAllSolutionAnswers()
        {
            using (var context = new AppDbContext())
            {
                var answers = context.SolutionAnswer;
                var results = new List<SolutionAnswerDTO>();
                foreach (var answer in answers)
                {
                    results.Add(Mapping.Mapper.Map<SolutionAnswerDTO>(answer));
                }

                return results;
            }
        }

        public List<SolutionAnswerDTO> GetSolutionAnswersByAnswer(int answerId)
        {
            using (var context = new AppDbContext())
            {                
                var solutionAnswers = context.SolutionAnswer
                    .Where(sA => sA.Answer.Id.Equals(answerId));
                var results = new List<SolutionAnswerDTO>();
                foreach(var answer in solutionAnswers)
                {
                    results.Add(Mapping.Mapper.Map<SolutionAnswerDTO>(results));
                }

                return results;
            }
        }
    }
}
