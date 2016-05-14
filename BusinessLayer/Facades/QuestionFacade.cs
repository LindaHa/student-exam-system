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
    public class QuestionFacade
    {
        public void CreateQuestion(QuestionDTO question)
        {
            Question newQuestion = Mapping.Mapper.Map<Question>(question);
            if (question.Area.Id > 0)
            {
                newQuestion.Area = null;
                newQuestion.AreaId = question.Area.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Question.Add(newQuestion);
                context.SaveChanges();
                //var myQuestion = context.Question
                //    .Include(q => q.Area)
                //    .SingleOrDefault(x => x.Id == question.Id);
            }

        }

        public void AddAnswer(int questionId, AnswerDTO answer )
        {
            var newAnswer = Mapping.Mapper.Map<Answer>(answer);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var question = context.Question.Find(questionId);
                question.Answers.Add(newAnswer);
                newAnswer.Question = question;
                context.SaveChanges();
            }
        }

        public void RemoveAnswer(int questionId, int answerId)
        {

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var question = context.Question
                    .Include(q => q.Answers).Include(q => q.Area)
                    .SingleOrDefault(q => q.Id == questionId);
                var answerFacade = new AnswerFacade();
                question.Answers.Remove(context.Answer.Find(answerId));
                answerFacade.DeleteAnswer(answerId);
                      
                context.SaveChanges();
            }
        }

        public void DeleteQuestion(int questionId)
        {
            using (var context = new AppDbContext())
            {
                var question = context.Question
                    .Include(q => q.Answers)
                    .Include(q => q.Area)
                    .SingleOrDefault(x => x.Id == questionId);
                if (question.Answers.Count > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (answers)");
                }
                context.Entry(question).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyQuestion(QuestionDTO question)
        {
            var newQuestion = Mapping.Mapper.Map<Question>(question);
            if (question.Area.Id > 0)
            {
                newQuestion.Area = null;
                newQuestion.AreaId = question.Area.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Entry(newQuestion).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public string GetExplanation(int questionId)
        {            
            using (var context = new AppDbContext())
            {
                var question = context.Question.Find(questionId);
                return question.Explanation;
            }
        }

        public List<QuestionDTO> GetAllQuestions()
        {
            using (var context = new AppDbContext())
            {
                var questions = context.Question.Include(q => q.Area);
                var results = new List<QuestionDTO>();
                foreach (var question in questions)
                {
                    results.Add(Mapping.Mapper.Map<QuestionDTO>(question));
                }
                return results;
            }
        }       

        public QuestionDTO GetQuestionById(int questionId)
        {
            using (var context = new AppDbContext())
            {
                var question = context.Question.Include(q => q.Answers)
                    .Include(q => q.Area)
                    .SingleOrDefault(x => x.Id == questionId);
                return Mapping.Mapper.Map<QuestionDTO>(question);
            }
        }

        public List<QuestionDTO> GetQuestionsByArea(int areaId)
        {
            using (var context = new AppDbContext())
            {
                var questions = context.Question.Include(q => q.Area)
                    .Where(q => q.Area.Id == areaId);
                var results = new List<QuestionDTO>();
                foreach (var question in questions)
                {
                    results.Add(Mapping.Mapper.Map<QuestionDTO>(questions));
                }
                return results;
            }
        }
    }
}
