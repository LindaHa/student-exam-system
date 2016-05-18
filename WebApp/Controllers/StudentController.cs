using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class StudentController : Controller
    {
        StudentFacade studentFacade = new StudentFacade();

        public ActionResult Show(int id)
        {
            var student = studentFacade.GetStudentById(id);
            return View(student);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["StudentTextWarning"];
            var model = studentFacade.GetAllStudents();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new StudentDTO());
        }

        [HttpPost]
        public ActionResult Create(StudentDTO student)
        {
            if (ModelState.IsValid)
            {
                StudentDTO newStudent = new StudentDTO();
                newStudent.FirstName = student.FirstName;
                newStudent.Surname = student.Surname;
                newStudent.Id = student.Id;
                newStudent.StudentGroups = student.StudentGroups;
                newStudent.Solutions = student.Solutions;

                studentFacade.CreateStudent(newStudent);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                studentFacade.DeleteStudent(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("StudentTextWarning", ex.Message);
            }
            return RedirectToAction("Index");


        }

        public ActionResult Edit(int id)
        {
            StudentDTO student = studentFacade.GetStudentById(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(int id, StudentDTO student)
        {
            if (ModelState.IsValid)
            {
                StudentDTO originalStudent = studentFacade.GetStudentById(id);
                originalStudent.FirstName = student.FirstName;
                originalStudent.Surname = student.Surname;
                originalStudent.Id = student.Id;
                originalStudent.StudentGroups = student.StudentGroups;
                originalStudent.Solutions = student.Solutions;

                studentFacade.ModifyStudent(originalStudent);

                return RedirectToAction("Show", new { id = originalStudent.Id });
            }
            return View(student);
        }
    }
}