using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class StudentGroupController : Controller
    {
        public StudentGroupFacade studentGroupFacade = new StudentGroupFacade();

        public ActionResult Show(int id)
        {
            var studentGroup = studentGroupFacade.GetStudentGroupById(id);
            return View(studentGroup);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["StudentGroupTextWarning"];
            var model = studentGroupFacade.GetAllStudentGroups();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new StudentGroupModel());
        }

        [HttpPost]
        public ActionResult Create(StudentGroupModel studentGroup)
        {
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();

                var newStudentGroup = new StudentGroupDTO();
                newStudentGroup.Id = studentGroup.Id;
                newStudentGroup.Code = studentGroup.Code;
                newStudentGroup.Students = studentGroup.Students;
                newStudentGroup.TestPatterns = studentGroup.TestPatterns;

                studentGroupFacade.CreateStudentGroup(newStudentGroup);
                return RedirectToAction("Index");
            }
            return View(studentGroup);
        }

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


        public ActionResult Edit(int id)
        {
            var studentGroup = studentGroupFacade.GetStudentGroupById(id);
            StudentGroupModel newStudentGroup = new StudentGroupModel();
            newStudentGroup.Id = studentGroup.Id;
            newStudentGroup.Code = studentGroup.Code;
            newStudentGroup.Students = studentGroup.Students;
            newStudentGroup.TestPatterns = studentGroup.TestPatterns;

            return View(newStudentGroup);
        }

        [HttpPost]
        public ActionResult Edit(int id, StudentGroupModel studentGroup)
        {
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();
                StudentGroupFacade StudentGroupFacade = new StudentGroupFacade();

                var originalStudentGroup = StudentGroupFacade.GetStudentGroupById(id);
                originalStudentGroup.Id = studentGroup.Id;
                originalStudentGroup.Students = studentGroup.Students;
                originalStudentGroup.TestPatterns = studentGroup.TestPatterns;

                StudentGroupFacade.ModifyStudentGroup(originalStudentGroup);

                return RedirectToAction("Show", new { id = originalStudentGroup.Id });
            }
            return View(studentGroup);
        }
    }
}