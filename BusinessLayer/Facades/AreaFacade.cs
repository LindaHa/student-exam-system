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
    public class AreaFacade
    {
        public void CreateArea(AreaDTO area)
        {
            var newArea = Mapping.Mapper.Map<Area>(area);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Area.Add(newArea);
                context.SaveChanges();
            }                
        }

        public void DeleteArea(int areaId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var area = context.Area
                    .Include(a => a.Questions)
                    .Include(a => a.TestPatterns)
                    .SingleOrDefault(a => a.Id == areaId);
                
                if(area.Questions.Count > 0 || area.TestPatterns.Count > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (questions or testPatterns)");
                }

                context.Entry(area).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyArea(AreaDTO area)
        {
            var newArea = Mapping.Mapper.Map<Area>(area);
            using (var context = new AppDbContext())
            {
                context.Entry(newArea).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void AddQuestion(int areaId, QuestionDTO question)
        {
            var newQuestion = Mapping.Mapper.Map<Question>(question);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var area = context.Area.Find(areaId);
                area.Questions.Add(newQuestion);
                newQuestion.Area = area;
                context.SaveChanges();
            }
        }

        public void RemoveQuestion(int areaId, int questionId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var question = context.Question.Find(questionId);
                var area = context.Area.Find(areaId);
                area.Questions.Remove(question);

                var questionFacade = new QuestionFacade();
                questionFacade.DeleteQuestion(questionId);
                context.SaveChanges();
            }
        }

        public AreaDTO GetAreaById(int areaId)
        {
            using (var context = new AppDbContext())
            {
                Area area = context.Area
                    .Include(a => a.Questions)
                    .Include(a => a.TestPatterns)
                    .SingleOrDefault(x => x.Id == areaId);
                return Mapping.Mapper.Map<AreaDTO>(area);
            }
        }

        public List<AreaDTO> GetAllArea()
        {
            using (var context = new AppDbContext())
            {
                var areas = context.Area;
                var results = new List<AreaDTO>();
                foreach (var area in areas)
                    results.Add(Mapping.Mapper.Map<AreaDTO>(area));
                return results;
            }
        }
    }
}
