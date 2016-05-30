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
    public class SolutionFacade
    {        
        public SolutionDTO CreateSolution(SolutionDTO solution)
        {
            var newSolution = Mapping.Mapper.Map<Solution>(solution);
            
            newSolution.Start = DateTime.Now;
            newSolution.IsDone = false;

            if(newSolution.Student.Id != 0)
            {
                newSolution.StudentId = newSolution.Student.Id;
                newSolution.Student = null;
            }
            if (newSolution.TestPattern.Id != 0)
            {
                newSolution.TestPatternId = newSolution.TestPattern.Id;
                newSolution.TestPattern = null;
            }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Solution.Add(newSolution);
                context.SaveChanges();
            }
            solution.Id = newSolution.Id;
            return solution;
        }
        
        public int GetPoints(SolutionDTO solution)
        {
            int result = 0;
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                List<SolutionAnswerDTO> solutionAnswers = solution.SolutionAnswers;

                foreach(SolutionAnswerDTO solutionAnswer in solutionAnswers)
                {
                    if (solutionAnswer != null && solutionAnswer.Answer.IsCorrect)
                        result += solutionAnswer.Answer.Question.PointsForCorrectAnswer;
                }
            }            
            return result;
        }

        public void AddAnswer(SolutionAnswerDTO solutionAnswer, int solutionId)
        {
            SolutionAnswer newAnswer = Mapping.Mapper.Map<SolutionAnswer>(solutionAnswer);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                Solution solution = context.Solution.Find(solutionId);
                //if (CheckIsNotDone(solution))
                //    throw new InvalidOperationException("The Test is finished");

                solution.SolutionAnswers.Add(newAnswer);
                context.SaveChanges();
            }
        }

        public void DeleteSolution(int solutionId)
        {
            using (var context = new AppDbContext())
            {
                var solution = context.Solution.Find(solutionId);
                if(solution.SolutionAnswers.Count > 0)
                {
                    throw new InvalidOperationException("There still are data present (solutions)");

                }
                solution.Student.Solutions.Remove(solution);
                context.Entry(solution).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifySolution(SolutionDTO solution)
        {
            var newSolution = Mapping.Mapper.Map<Solution>(solution);
            if (solution.Student.Id > 0)
            {
                newSolution.Student = null;
                newSolution.StudentId = solution.Student.Id;
            }
            if (solution.TestPattern.Id > 0)
            {
                newSolution.TestPattern = null;
                newSolution.TestPatternId = solution.TestPattern.Id;
            }
            using (var context = new AppDbContext())
            {
                context.Entry(newSolution).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void RemoveAnswer(int solutionAnswerId, int solutionId)
        {

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solution = context.Solution.Find(solutionId);                

                solution.SolutionAnswers.RemoveAt(solutionAnswerId);
                context.SaveChanges();
            }
        }

        public SolutionDTO GetSolutionById(int solutionId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solution = context.Solution.Include(s => s.Student)
                    .Include(s => s.TestPattern).SingleOrDefault(s => s.Id == solutionId);
                return Mapping.Mapper.Map<SolutionDTO>(solution);
            }
        }

        public List<SolutionDTO> GetSolutionsOfStudentGroup(int studentGroupId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var enrollments = context.Enrollment
                        .Include(e => e.Student.Solutions)
                        .Where(p => p.StudentGroupId == studentGroupId);

                List<SolutionDTO> results = new List<SolutionDTO>();
                foreach (var enrollment in enrollments)
                    foreach(var solution in enrollment.Student.Solutions)
                        results.Add(Mapping.Mapper.Map<SolutionDTO>(solution));

                return results;
            }
        }

        public List<SolutionDTO> GetSolutionsByTestPattern(int testPatternId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solutions = context.Solution
                        .Where(s => s.TestPattern.Id.Equals(testPatternId));

                List<SolutionDTO> results = new List<SolutionDTO>();
                foreach (var item in solutions)
                    results.Add(Mapping.Mapper.Map<SolutionDTO>(item));

                return results;
            }
        }

        public List<SolutionDTO> GetAllSolutions()
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solutions = context.Solution;

                List<SolutionDTO> results = new List<SolutionDTO>();
                foreach (var item in solutions)
                    results.Add(Mapping.Mapper.Map<SolutionDTO>(item));

                return results;
            }
        }

        public void FinishSolution(SolutionDTO solution)
        {
            solution.IsDone = true;
            solution.End = DateTime.Now.Date;
        }
    }
}
