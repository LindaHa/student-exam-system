namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Entity;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.AppDbContext context)
        {
            context.Area.AddOrUpdate(
                a => a.Id,
                new Area { Id = 1, Name = "Applied Informatics" },
                new Area { Id = 2, Name = "Graphics" });

            context.Question.AddOrUpdate(
                q => q.Id,
                new Question { Id = 1, Text = "Where are we?", PointsForCorrectAnswer = 1, AreaId = 1, Explanation = "No further explanation", TypeOfQuestion = "single" },
                new Question { Id = 2, Text = "2+2", PointsForCorrectAnswer = 1, AreaId = 1, Explanation = "No further explanation", TypeOfQuestion = "single" },
                new Question { Id = 3, Text = "Select the truths", PointsForCorrectAnswer = 1, AreaId = 1, Explanation = "No further explanation", TypeOfQuestion = "multiple" });

            context.Answer.AddOrUpdate(
                a => a.Id,
                new Answer { Id = 1, IsCorrect = true, Text = "Brno", QuestionId = 1 },
                new Answer { Id = 2, IsCorrect = false, Text = "Munchen", QuestionId = 1 },
                new Answer { Id = 3, IsCorrect = false, Text = "Chicago", QuestionId = 1 },
                new Answer { Id = 4, IsCorrect = true, Text = "4", QuestionId = 2 },
                new Answer { Id = 5, IsCorrect = false, Text = "infinity", QuestionId = 2 },
                new Answer { Id = 6, IsCorrect = true, Text = "1", QuestionId = 3 },
                new Answer { Id = 7, IsCorrect = false, Text = "false", QuestionId = 3 },
                new Answer { Id = 8, IsCorrect = true, Text = "true", QuestionId = 3 },
                new Answer { Id = 9, IsCorrect = true, Text = "-1", QuestionId = 3 },
                new Answer { Id = 10, IsCorrect = false, Text = "0", QuestionId = 3 });

            context.Teacher.AddOrUpdate(
                c => c.Id,
                new Teacher { Id = 1, FirstName = "Jon", Surname = "Doe" });

            context.StudentGroup.AddOrUpdate(
                s => s.Id,
                new StudentGroup { Id = 1, Code = "theone", TeacherId = 1});

            context.TestPattern.AddOrUpdate(
                t => t.Id,
                new TestPattern { Id = 1, AreaId = 1, Name = "The 3 questions", TeacherId = 1, NumberOfQuestions = 3, Time = 5, StudentGroupId = 1, Start = DateTime.Now, End = DateTime.Now.AddDays(5)});
        }
    }
}
