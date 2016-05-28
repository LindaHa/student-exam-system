using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        TeacherFacade teacherFacade = new TeacherFacade();

        public ActionResult Show(int id)
        {
            var teacher = teacherFacade.GetTeacherById(id);
            return View(teacher);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["TeacherTextWarning"];
            var model = teacherFacade.GetAllTeachers();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new TeacherDTO());
        }

        [HttpPost]
        public ActionResult Create(TeacherDTO teacher)
        {
            if (ModelState.IsValid)
            {
                TeacherDTO newTeacher = new TeacherDTO();
                newTeacher.FirstName = teacher.FirstName;
                newTeacher.Surname = teacher.Surname;
                newTeacher.Id = teacher.Id;
                newTeacher.TestPatterns = teacher.TestPatterns;
                newTeacher.StudentGroups = teacher.StudentGroups;

                teacherFacade.CreateTeacher(newTeacher);
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                teacherFacade.DeleteTeacher(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("TeacherTextWarning", ex.Message);
            }
            return RedirectToAction("Index");


        }

        public ActionResult Edit(int id)
        {
            var teacher = teacherFacade.GetTeacherById(id);
            return View(teacher);
        }

        [HttpPost]
        public ActionResult Edit(int id, TeacherDTO teacher)
        {
            if (ModelState.IsValid)
            {
                var originalTeacher = teacherFacade.GetTeacherById(id);
                originalTeacher.FirstName = teacher.FirstName;
                originalTeacher.Surname = teacher.Surname;
                originalTeacher.Id = teacher.Id;
                originalTeacher.TestPatterns = teacher.TestPatterns;
                originalTeacher.StudentGroups = teacher.StudentGroups;

                teacherFacade.ModifyTeacher(originalTeacher);

                return RedirectToAction("Show", new { id = originalTeacher.Id });
            }
            return View(teacher);
        }
    }
}