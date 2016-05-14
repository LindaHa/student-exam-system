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
    public class AnswerFacade
    {
        public void CreateAnswer(AnswerDTO answer)
        {
            Answer newAnswer = Mapping.Mapper.Map<Answer>(answer);
            if (answer.Question.Id > 0)
            {
                newAnswer.Question = null;
                newAnswer.QuestionId = answer.Question.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Answer.Add(newAnswer);
                context.SaveChanges();
            }
        }

        public void DeleteAnswer(int answerId)
        {
            using (var context = new AppDbContext())
            {
                var answer = context.Answer.Find(answerId);
                var solutionAnswerFacade = new SolutionAnswerFacade();
                if(solutionAnswerFacade.GetSolutionAnswersByAnswer(answerId).Count > 0)
                    throw new InvalidOperationException
                        ("There still are data present (solutionAnswers)");
                context.Entry(answer).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyAnswer(AnswerDTO answer)
        {
            var newAnswer = Mapping.Mapper.Map<Answer>(answer);
            if (answer.Question.Id > 0)
            {
                newAnswer.Question = null;
                newAnswer.QuestionId = answer.Question.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Entry(newAnswer).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public List<AnswerDTO> GetAllAnswers()
        {
            using (var context = new AppDbContext())
            {
                var answers = context.Answer.Include(a => a.Question);
                var results = new List<AnswerDTO>();
                foreach (var answer in answers)
                    results.Add(Mapping.Mapper.Map<AnswerDTO>(answer));

                return results;
            }
        }

        //public List<AnswerDTO> GetSolutionAnswersByQuestion(int questionId)
        //{
        //    using (var context = new AppDbContext())
        //    {
        //        var answers = context.Answer
        //            .Where(sA => sA.Question.Id.Equals(questionId));
        //        var results = new List<AnswerDTO>();
        //        foreach (var answer in answers)
        //        {
        //           results.Add(Mapping.Mapper.Map<AnswerDTO>(results));
        //        }
        //        return results;
        //    }
        //}

        public AnswerDTO GetAnswerById(int answerId)
        {
            using (var context = new AppDbContext())
            {
                var answer = context.Answer
                    .Include(a => a.Question)
                    .SingleOrDefault(x => x.Id == answerId);
                return Mapping.Mapper.Map<AnswerDTO>(answer);
            }
        }
    }
}
