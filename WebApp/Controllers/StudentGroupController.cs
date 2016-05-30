using BL.Facades;
using BusinessLayer.DTO;
using BusinessLayer.Facades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class StudentGroupController : Controller
    {
        public StudentGroupFacade studentGroupFacade = new StudentGroupFacade();

        [Authorize(Roles = "Admin, Student, Teacher")]
        public ActionResult Show(int id)
        {
            var studentGroup = studentGroupFacade.GetStudentGroupById(id);
            studentGroup.Code = "";
            return View(studentGroup);
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpPost]
        public ActionResult Show(int id, StudentGroupDTO studentGroup)
        {
            UserFacade userFacade = new UserFacade();
            UserDTO user = userFacade.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));
            StudentDTO student = user.Student;
            StudentGroupDTO myStudentGroup = studentGroupFacade.GetStudentGroupById(id);

            try {
                studentGroupFacade.AddStudent(id, student, studentGroup.Code);
                TempData["SolutionTextWarning"] = "You are in!";
                return RedirectToAction("Index", "Solution");
            }
            catch(InvalidOperationException e)
            {
                myStudentGroup.Code = "";
                ModelState.AddModelError("Code", e.Message);
                return View(myStudentGroup);
            }
        }

        [Authorize(Roles = "Admin, Student, Teacher")]
        public ActionResult Index()
        {
            ViewBag.Warning = TempData["StudentGroupTextWarning"];
            var model = studentGroupFacade.GetAllStudentGroups();
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Create()
        {
            return View(new StudentGroupModel());
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public ActionResult Create(StudentGroupModel studentGroup)
        {
            UserFacade userFacade = new UserFacade();
            var user = userFacade.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();

                var newStudentGroup = new StudentGroupDTO();
                newStudentGroup.Id = studentGroup.Id;
                newStudentGroup.Code = studentGroup.Code;
                newStudentGroup.TestPatterns = studentGroup.TestPatterns;
                newStudentGroup.Teacher = user.Teacher;

                studentGroupFacade.CreateStudentGroup(newStudentGroup);
                return RedirectToAction("Index");
            }
            return View(studentGroup);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Delete(int id)
        {
            try
            {
                studentGroupFacade.DeleteStudentGroup(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("StudentGroupTextWarning", ex.Message);
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Edit(int id)
        {
            var studentGroup = studentGroupFacade.GetStudentGroupById(id);
            StudentGroupModel newStudentGroup = new StudentGroupModel();
            newStudentGroup.Id = studentGroup.Id;
            newStudentGroup.Code = studentGroup.Code;
            newStudentGroup.Teacher = studentGroup.Teacher;
            newStudentGroup.TestPatterns = studentGroup.TestPatterns;

            return View(newStudentGroup);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public ActionResult Edit(int id, StudentGroupModel studentGroup)
        {
            if (ModelState.IsValid)
            {
                var originalStudentGroup = studentGroupFacade.GetStudentGroupById(id);
                originalStudentGroup.Id = studentGroup.Id;
                originalStudentGroup.TestPatterns = studentGroup.TestPatterns;

                studentGroupFacade.ModifyStudentGroup(originalStudentGroup);

                return RedirectToAction("Show", new { id = originalStudentGroup.Id });
            }
            return View(studentGroup);
        }        
    }
}