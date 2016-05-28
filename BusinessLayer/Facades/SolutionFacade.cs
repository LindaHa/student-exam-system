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
        //public Solution solution = new Solution();

        //zkontroluje, zda student muze s testem pracovat
        private bool CheckIsNotDone(SolutionDTO solution)
        {
            var newSolution = Mapping.Mapper.Map<Solution>(solution);
            int time = Convert.ToInt32(newSolution.End - newSolution.Start);
            return ((time < newSolution.TestPattern.Time) && 
                    (DateTime.Now.Date < newSolution.TestPattern.End) &&
                    (!newSolution.IsDone));  
        }

        private bool CheckIsNotDone(Solution solution)
        {
            int time = Convert.ToInt32(solution.End - solution.Start);
            return ((time < solution.TestPattern.Time) &&
                    (DateTime.Now.Date < solution.TestPattern.End) &&
                    (!solution.IsDone));
        }

        public SolutionDTO CreateSolution(SolutionDTO solution)
        {
            var newSolution = Mapping.Mapper.Map<Solution>(solution);
            
            newSolution.Start = DateTime.Now.Date;
            newSolution.IsDone = false;

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
            if (!CheckIsNotDone(solution))
                throw new InvalidOperationException("The Test is not finished yet.");

            int result = 0;
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solutionAnswers = solution.SolutionAnswers;

                foreach(var solutionAnswer in solutionAnswers)
                {
                    if (solutionAnswer != null && solutionAnswer.Answer.IsCorrect)
                        result += solutionAnswer.Answer.Question.PointsForCorrectAnswer;
                }
            }            
            return result;
        }

        public void AddAnswer(SolutionAnswerDTO solutionAnswer, int solutionId)
        {
            var newAnswer = Mapping.Mapper.Map<SolutionAnswer>(solutionAnswer);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solution = context.Solution.Find(solutionId);
                if (CheckIsNotDone(solution))
                    throw new InvalidOperationException("The Test is finished");

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
                if (!CheckIsNotDone(solution))
                    throw new InvalidOperationException("The Test is finished");

                solution.SolutionAnswers.RemoveAt(solutionAnswerId);
                context.SaveChanges();
            }
        }

        public SolutionDTO GetSolutionById(int solutionId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solution = context.Solution.Find(solutionId);
                return Mapping.Mapper.Map<SolutionDTO>(solution);
            }
        }

        public List<SolutionDTO> GetSolutionsOfStudentGroup(int studentGroupId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var solutions = context.Solution
                        .GroupBy(s => s.Student.StudentGroups)
                        .Select(g => g.Key.Equals(studentGroupId));

                List<SolutionDTO> results = new List<SolutionDTO>();
                foreach (var item in solutions)
                    results.Add(Mapping.Mapper.Map<SolutionDTO>(item));

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
            if (!CheckIsNotDone(solution))
                throw new InvalidOperationException("The Test is already finished");

            solution.IsDone = true;
            solution.End = DateTime.Now.Date;
        }
    }
}
