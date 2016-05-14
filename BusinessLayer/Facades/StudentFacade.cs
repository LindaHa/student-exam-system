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
    public class StudentFacade
    {
        public void CreateStudent(StudentDTO student)
        {
            var newStudent = Mapping.Mapper.Map<Student>(student);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Student.Add(newStudent);
                context.SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var context = new AppDbContext())
            {
                var student = context.Student
                    .Include(s => s.Solutions).Include(s => s.StudentGroups)
                    .SingleOrDefault(s => s.Id == studentId);
                if(student.Solutions.Count > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (solutions)");
                }
                if (student.StudentGroups != null)
                {
                    foreach(var group in student.StudentGroups)
                        group.Students.Remove(student);                    
                }

                context.Entry(student).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyStudent(StudentDTO student)
        {
            var newStudent = Mapping.Mapper.Map<Student>(student);
            using (var context = new AppDbContext())
            {
                context.Entry(newStudent).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public StudentDTO GetStudentById(int studentId)
        {
            using (var context = new AppDbContext())
            {
                var student = context.Student
                    .Include(s => s.Solutions).Include(s => s.StudentGroups)
                    .SingleOrDefault(s => s.Id == studentId);
                return Mapping.Mapper.Map<StudentDTO>(student);
            }
        }

        public List<StudentDTO> GetAllStudents()
        {
            using (var context = new AppDbContext())
            {
                var students = context.Student
                    .Include(s => s.Solutions).Include(s => s.StudentGroups);
                var results = new List<StudentDTO>();
                foreach (var student in students)
                    results.Add(Mapping.Mapper.Map<StudentDTO>(student));
                return results;
            }
        }

        public void AddStudentGroup(int studentGroupId, StudentDTO student, string code)
        {
            var newStudent = Mapping.Mapper.Map<Student>(student);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var group = context.StudentGroup
                    .Include(s => s.TestPatterns).Include(s => s.Students)
                    .SingleOrDefault(s => s.Id == studentGroupId);
                if (!code.Equals(group.Code))
                {
                    throw new InvalidOperationException("Invalid Code");
                }
                group.Students.Add(newStudent);
                newStudent.StudentGroups.Add(group);
                context.SaveChanges();
            }
        }

        public void RemoveStudentGroup(int studentGroupId, int studentId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var group = context.StudentGroup
                    .Include(g => g.Students).Include(g => g.TestPatterns)
                    .SingleOrDefault(g => g.Id == studentGroupId);
                var student = context.Student
                    .Include(s => s.Solutions).Include(s => s.StudentGroups)
                    .SingleOrDefault(s => s.Id == studentId);
                group.Students.Remove(student);
                student.StudentGroups.Remove(group);
                context.SaveChanges();
            }
        }

    }
}
