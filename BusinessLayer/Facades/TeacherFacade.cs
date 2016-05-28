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
    public class TeacherFacade
    {
        public void CreateTeacher(TeacherDTO teacher)
        {
            var newTeacher = Mapping.Mapper.Map<Teacher>(teacher);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Teacher.Add(newTeacher);
                context.SaveChanges();
            }
        }

        public void DeleteTeacher(int teacherId)
        {
            using (var context = new AppDbContext())
            {
                var teacher = context.Teacher
                    .Include(t => t.TestPatterns).Include(t => t.StudentGroups)
                    .SingleOrDefault(t => t.Id == teacherId);
                if (teacher.TestPatterns.Count > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (testPatterns)");
                }
                if (teacher.StudentGroups.Count > 0)
                {
                    throw new InvalidOperationException
                        ("There still are data present (studentGroups)");
                }

                context.Entry(teacher).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void ModifyTeacher(TeacherDTO teacher)
        {
            var newTeacher = Mapping.Mapper.Map<Teacher>(teacher);
            using (var context = new AppDbContext())
            {
                context.Entry(newTeacher).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public TeacherDTO GetTeacherById(int teacherId)
        {
            using (var context = new AppDbContext())
            {
                var teacher = context.Teacher
                    .Include(t => t.TestPatterns).Include(s => s.StudentGroups)
                    .SingleOrDefault(t => t.Id == teacherId);
                return Mapping.Mapper.Map<TeacherDTO>(teacher);
            }
        }

        public List<TeacherDTO> GetAllTeachers()
        {
            using (var context = new AppDbContext())
            {
                var teachers = context.Teacher
                    .Include(t => t.TestPatterns).Include(s => s.StudentGroups);
                var results = new List<TeacherDTO>();
                foreach (var teacher in teachers)
                    results.Add(Mapping.Mapper.Map<TeacherDTO>(teacher));

                return results;
            }
        }

        public int GetNumberOfTestPatterns(int teacherId)
        {
            using (var context = new AppDbContext())
            {
                var teacher = context.Teacher
                    .Include(t => t.TestPatterns)
                    .SingleOrDefault(t => t.Id == teacherId);
                return teacher.TestPatterns.Count();
            }
        }

        public int GetNumberOfStudentGroups(int teacherId)
        {
            using (var context = new AppDbContext())
            {
                Teacher teacher = context.Teacher
                    .Include(t => t.StudentGroups)
                    .SingleOrDefault(t => t.Id == teacherId);
                return teacher.StudentGroups.Count();
            }
        }
    }
}
