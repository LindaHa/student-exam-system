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
    public class StudentGroupFacade
    {
        public void CreateStudentGroup(StudentGroupDTO studentGroup)
        {
            StudentGroup newGroup = Mapping.Mapper.Map<StudentGroup>(studentGroup);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.StudentGroup.Add(newGroup);
                context.SaveChanges();
            }
        }

        public void AddStudent(int studentGroupId, StudentDTO student, string code)
        {                  
            Student newStudent = Mapping.Mapper.Map<Student>(student);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                StudentGroup group = context.StudentGroup
                    .Include(s => s.Students).Include(s => s.TestPatterns).Include(s => s.Teacher)
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

        public void RemoveStudent(int studentGroupId, int studentId)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                StudentGroup group = context.StudentGroup
                    .Include(g => g.Students).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .SingleOrDefault(g => g.Id == studentGroupId);
                Student student = context.Student
                    .Include(s => s.StudentGroups).Include(s => s.Solutions)
                    .SingleOrDefault(s => s.Id == studentId);
                group.Students.Remove(student);
                student.StudentGroups.Remove(group);
                context.SaveChanges();
            }
        }

        public void DeleteStudentGroup(int studentGroupId)
        {            
            using (var context = new AppDbContext())
            {
                StudentGroup group = context.StudentGroup
                    .Include(g => g.Students).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .SingleOrDefault(g => g.Id == studentGroupId);
                foreach (var student in group.Students)
                    student.StudentGroups.Remove(group);

                foreach (var pattern in group.TestPatterns)
                    pattern.StudentGroup = null;
                group.Teacher.StudentGroups.Remove(group);

                context.Entry(group).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyStudentGroup(StudentGroupDTO studentGroup)
        {
            var newGroup = Mapping.Mapper.Map<StudentGroup>(studentGroup);
            using (var context = new AppDbContext())
            {
                context.Entry(newGroup).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public StudentGroupDTO GetStudentGroupById(int studentGroupId)
        {
            using (var context = new AppDbContext())
            {
                var group = context.StudentGroup
                    .Include(g => g.Students).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .SingleOrDefault(g => g.Id == studentGroupId);
                return Mapping.Mapper.Map<StudentGroupDTO>(group);
            }
        }

        public List<StudentGroupDTO> GetAllStudentGroups()
        {
            using (var context = new AppDbContext())
            {
                var groups = context.StudentGroup
                    .Include(g => g.Students).Include(g => g.TestPatterns).Include(s => s.Teacher);
                var results = new List<StudentGroupDTO>();
                foreach (var group in groups)
                    results.Add(Mapping.Mapper.Map<StudentGroupDTO>(groups));

                return results;
            }
        }

        public StudentGroupDTO GetStudentGroupByStudent(int studentId)
        {
            using (var context = new AppDbContext())
            {
                var group = context.StudentGroup
                    .Include(g => g.Students).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .Select(g => g.Students
                                .Where(s => s.Id.Equals(studentId)));

                return Mapping.Mapper.Map<StudentGroupDTO>(group);
            }
        }
    }
}
