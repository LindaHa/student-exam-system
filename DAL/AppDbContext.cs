using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using DAL.IdentityEntities;

namespace DAL
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public AppDbContext() : base("StudentExamSystem")
        {

        }
        public DbSet<Area> Area { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentGroup> StudentGroup { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<TestPattern> TestPattern { get; set; }
        public DbSet<Solution> Solution { get; set; }
        public DbSet<SolutionAnswer> SolutionAnswer { get; set; }
        public DbSet<SolutionQuestion> SolutionQuestion { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        //public AppDbContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DAL.AppDbContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        //{
        //    Database.SetInitializer(new AppDbInitializer());
        //}
    }

    //public class AppDbInitializer : DropCreateDatabaseAlways<AppDbContext>
    //{
    //    protected override void Seed(AppDbContext context)
    //    {
    //        Ukázka inicializace dat
    //       Area PB164 = new Area() { Name = "Programing Java" };
    //        context.Area.Add(PB164);

    //        Question question1 = (new Question()
    //        {
    //            Text = "Is Java OOP?",
    //            TypeOfQuestion = "SingleValue",
    //            Answers = new List<Answer>(),
    //            PointsForCorrectAnswer = 2,
    //            Area = PB164
    //        });

    //        Answer answer1 = new Answer() { Text = "Yes", IsCorrect = true, Question = question1 };
    //        Answer answer2 = new Answer() { Text = "No", IsCorrect = false, Question = question1 };

    //        question1.Answers.Add(answer1);
    //        question1.Answers.Add(answer2);

    //        PB164.Questions.Add(question1);

    //        base.Seed(context);
    //    }
    //}
}
