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
            Enrollment enrollment = new Enrollment();
            enrollment.StudentGroupId = studentGroupId;
            enrollment.StudentId = student.Id;

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                StudentGroup group = context.StudentGroup.SingleOrDefault(s => s.Id == studentGroupId);
                if (!code.Equals(group.Code))
                {
                    throw new InvalidOperationException("Invalid Code");
                }
                context.Enrollment.Add(enrollment);
                context.SaveChanges();
            }
        }

        public void DeleteStudentGroup(int studentGroupId)
        {            
            using (var context = new AppDbContext())
            {
                StudentGroup group = context.StudentGroup
                    .Include(g => g.Enrollments).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .SingleOrDefault(g => g.Id == studentGroupId);
                foreach (var enrollment in group.Enrollments)
                    context.Entry(enrollment).State = EntityState.Deleted;

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
                    .Include(g => g.Enrollments.Select(e => e.Student)).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .SingleOrDefault(g => g.Id == studentGroupId);
                return Mapping.Mapper.Map<StudentGroupDTO>(group);
            }
        }

        public List<StudentGroupDTO> GetAllStudentGroups()
        {
            using (var context = new AppDbContext())
            {
                List<StudentGroup> groups = context.StudentGroup
                    .Include(g => g.Enrollments).Include(g => g.TestPatterns).Include(s => s.Teacher)
                    .ToList();
                List<StudentGroupDTO> results = new List<StudentGroupDTO>();
                foreach (StudentGroup group in groups)
                    results.Add(Mapping.Mapper.Map<StudentGroupDTO>(group));

                return results;
            }
        }
    }
}
