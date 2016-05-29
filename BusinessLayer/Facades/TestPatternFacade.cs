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
    public class TestPatternFacade
    {
        public void CreateTestPattern(TestPatternDTO testPattern)
        {
            var newPattern = Mapping.Mapper.Map<TestPattern>(testPattern);
            if (testPattern.Area.Id > 0)
            {
                newPattern.Area = null;
                newPattern.AreaId = testPattern.Area.Id;
            }
            if (testPattern.Teacher.Id > 0)
            {
                newPattern.Teacher = null;
                newPattern.TeacherId = testPattern.Teacher.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.TestPattern.Add(newPattern);
                context.SaveChanges();
                var myPattern = context.TestPattern
                    .Include(p => p.Area).Include(p => p.Teacher).Include(p => p.StudentGroup)
                    .SingleOrDefault(x => x.Id == testPattern.Id);
            }
        }

        public void AddStudentGroup(int testPatternId, StudentGroupDTO studentGroup)
        {
            var newGroup = Mapping.Mapper.Map<StudentGroup>(studentGroup);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var pattern = context.TestPattern
                    .Include(p => p.StudentGroup).Include(p => p.Teacher).Include(p => p.Area)
                    .SingleOrDefault(p => p.Id == testPatternId);
                if (pattern.StudentGroupId != 0)
                    throw new InvalidOperationException
                        ("This test already has a studentGroup. Please remove it before adding another one.");
                pattern.StudentGroup = newGroup;
                newGroup.TestPatterns.Add(pattern);
                context.SaveChanges();
            }
        }

        public void RemoveStudentGroup(int testPatternId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var pattern = context.TestPattern
                    .SingleOrDefault(p => p.Id == testPatternId);
                pattern.StudentGroup = null;
                pattern.StudentGroupId = 0;

                context.SaveChanges();
            }
        }

        public void DeleteTestPattern(int testPatternId)
        {
            using (var context = new AppDbContext())
            {
                var solutions = context.Solution
                    .Include(s => s.TestPattern)
                    .Where(s => s.TestPatternId == testPatternId);

                var pattern = context.TestPattern
                    .Include(q => q.Teacher)
                    .Include(q => q.Area)
                    .SingleOrDefault(x => x.Id == testPatternId);
                if (solutions.Count() > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (solutions)");
                }
                context.Entry(pattern).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyTestPattern(TestPatternDTO testPattern)
        {
            var newPattern = Mapping.Mapper.Map<TestPattern>(testPattern);
            if (testPattern.Area.Id > 0)
            {
                newPattern.Area = null;
                newPattern.AreaId = testPattern.Area.Id;
            }
            if (testPattern.Teacher.Id > 0)
            {
                newPattern.Teacher = null;
                newPattern.TeacherId = testPattern.Teacher.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Entry(newPattern).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public List<QuestionDTO> GetTestQuestions(int testPatternId)
        {
            using(var context = new AppDbContext())
            {
                var pattern = context.TestPattern
                    .Include(p => p.Teacher)
                    .Include(p => p.Area)
                    .Include(p => p.Area.Questions)
                    .Include(p => p.Area.Questions.Select(q => q.Answers))
                    .SingleOrDefault(x => x.Id == testPatternId);
                int neededQuestions = pattern.NumberOfQuestions;
                List<Question> questions = pattern.Area.Questions;
                var test = new List<QuestionDTO>();

                var results = questions.OrderBy(x => (new Random()).Next())                    
                    .Take(neededQuestions < questions.Count() ? neededQuestions : questions.Count());

                //return Mapping.Mapper.Map <List<QuestionDTO>>(results);

                foreach (var result in results)
                    test.Add(Mapping.Mapper.Map<QuestionDTO>(result));

                return test;
            }
        }

       
        public TestPatternDTO GetTestPatternById(int testPatternId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var pattern = context.TestPattern
                    .Include(p => p.Area).Include(p => p.Teacher).Include(p => p.StudentGroup)
                    .SingleOrDefault(p => p.Id == testPatternId);
                return Mapping.Mapper.Map<TestPatternDTO>(pattern);
            }
        }

        public List<TestPatternDTO> GetTestPatternsByGroup(int studentGroupId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var patterns = context.TestPattern
                        .Include(p => p.Area).Include(p => p.Teacher).Include(p => p.StudentGroup)
                        .Where(p => p.StudentGroupId == studentGroupId);

                List<TestPatternDTO> results = new List<TestPatternDTO>();
                foreach (var item in patterns)
                    results.Add(Mapping.Mapper.Map<TestPatternDTO>(item));

                return results;
            }
        }

        public List<TestPatternDTO> GetAllTestPatterns()
        {
            using(var context = new AppDbContext())
            {
                var patterns = context.TestPattern
                    .Include(p => p.Area).Include(p => p.Teacher).Include(p => p.StudentGroup);
                var results = new List<TestPatternDTO>();

                foreach (var item in patterns)
                    results.Add(Mapping.Mapper.Map<TestPatternDTO>(item));

                return results;
            }
        }


    }
}
