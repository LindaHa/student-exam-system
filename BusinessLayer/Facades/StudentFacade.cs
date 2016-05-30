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
                    .Include(s => s.Solutions).Include(s => s.Enrollments)
                    .SingleOrDefault(s => s.Id == studentId);
                if(student.Solutions.Count > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (solutions)");
                }
                if (student.Enrollments != null)
                {
                    foreach(var enrollment in student.Enrollments)
                        context.Entry(enrollment).State = EntityState.Deleted;
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
                    .Include(s => s.Solutions).Include(s => s.Enrollments)
                    .SingleOrDefault(s => s.Id == studentId);
                return Mapping.Mapper.Map<StudentDTO>(student);
            }
        }

        public List<StudentGroupDTO> GetStudentStudentGroups(int studentId)
        {
            using (var context = new AppDbContext())
            {
                var enrollments = context.Enrollment.Include(e => e.StudentGroup).Where(e => e.StudentId == studentId);

                List<StudentGroupDTO> studentGroups = new List<StudentGroupDTO>();
                foreach (var enrollment in enrollments)
                    studentGroups.Add(Mapping.Mapper.Map<StudentGroupDTO>(enrollment.StudentGroup));

                return studentGroups;
            }
        }

        public List<StudentDTO> GetAllStudents()
        {
            using (var context = new AppDbContext())
            {
                var students = context.Student
                    .Include(s => s.Solutions);
                var results = new List<StudentDTO>();
                foreach (var student in students)
                    results.Add(Mapping.Mapper.Map<StudentDTO>(student));
                return results;
            }
        }

        public void AddStudentGroup(int studentGroupId, StudentDTO student, string code)
        {
            Enrollment enrollment = new Enrollment();
            enrollment.StudentGroupId = studentGroupId;
            enrollment.StudentId = student.Id;

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var group = context.StudentGroup.SingleOrDefault(g => g.Id == studentGroupId);
                if (!code.Equals(group.Code))
                {
                    throw new InvalidOperationException("Invalid Code");
                }
                context.Enrollment.Add(enrollment);
                context.SaveChanges();
            }
        }

        public void RemoveStudentGroup(int studentGroupId, int studentId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var enrollment = context.Enrollment.SingleOrDefault(s => s.StudentGroupId == studentGroupId && s.StudentId == studentId);
                context.Entry(enrollment).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

    }
}
