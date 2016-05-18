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
    //[Authorize(Roles = "teacher, admin")]
    public class TestPatternController : Controller
    {
        public TestPatternFacade testPatternFacade = new TestPatternFacade();

        public ActionResult Show(int id)
        {
            var pattern = testPatternFacade.GetTestPatternById(id);
            return View(pattern);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["TestPatternTextWarning"];
            var model = testPatternFacade.GetAllTestPatterns();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new TestPatternModel());
        }

        [HttpPost]
        public ActionResult Create(TestPatternModel testPattern)
        {
            UserFacade userFacade = new UserFacade();
            var user = userFacade.GetUserById
                (Convert.ToInt32(User.Identity.GetUserId()));
            //UserDTO currentUser = userFacade.
            //testPattern.SelectedTeacherId <-sem pripradit id pouzivatela
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();
                TeacherFacade teacherFacade = new TeacherFacade();
                TestPatternFacade patternFacade = new TestPatternFacade();
                StudentGroupFacade groupFacade = new StudentGroupFacade();

                var newPattern = new TestPatternDTO();
                newPattern.Area = areaFacade.GetAreaById(testPattern.SelectedAreaId);
                newPattern.Teacher = user.Teacher;
                newPattern.Id = testPattern.Id;
                newPattern.Name = testPattern.Name;
                newPattern.NumberOfQuestions = testPattern.NumberOfQuestions;
                newPattern.Start = testPattern.Start;
                newPattern.StudentGroup = groupFacade.GetStudentGroupById
                    (testPattern.SelectedStudentGroupId);
                newPattern.Time = testPattern.Time;
                newPattern.End = testPattern.End;

                patternFacade.CreateTestPattern(newPattern);
                return RedirectToAction("Index");
            }
            return View(testPattern);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                testPatternFacade.DeleteTestPattern(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("TestPatternTextWarning", ex.Message);
            }
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var pattern = testPatternFacade.GetTestPatternById(id);
            TestPatternModel newPattern = new TestPatternModel();
            newPattern.SelectedAreaId = pattern.Area.Id;
            newPattern.SelectedTeacherId = pattern.Teacher.Id;
            newPattern.Id = pattern.Id;
            newPattern.Name = pattern.Name;
            newPattern.NumberOfQuestions = pattern.NumberOfQuestions;
            newPattern.Start = pattern.Start;
            newPattern.SelectedStudentGroupId = pattern.StudentGroup.Id;
            newPattern.Time = pattern.Time;
            newPattern.End = pattern.End;

            return View(newPattern);
        }

        [HttpPost]
        public ActionResult Edit(int id, TestPatternModel testPattern)
        {
            if (ModelState.IsValid)
            {
                AreaFacade areaFacade = new AreaFacade();
                TestPatternFacade patternFacade = new TestPatternFacade();
                TeacherFacade teacherFacade = new TeacherFacade();
                StudentGroupFacade groupFacade = new StudentGroupFacade();

                var originalPattern = patternFacade.GetTestPatternById(id);
                originalPattern.End = testPattern.End;
                originalPattern.Area = areaFacade.GetAreaById(testPattern.SelectedAreaId);
                originalPattern.Name = testPattern.Name;
                originalPattern.NumberOfQuestions = testPattern.NumberOfQuestions;
                originalPattern.Start = testPattern.Start;
                originalPattern.StudentGroup = groupFacade
                    .GetStudentGroupById(testPattern.SelectedStudentGroupId);
                originalPattern.Teacher = teacherFacade
                    .GetTeacherById(testPattern.SelectedTeacherId);
                originalPattern.Time = testPattern.Time;

                patternFacade.ModifyTestPattern(originalPattern);

                return RedirectToAction("Show", new { id = originalPattern.Id });
            }
            return View(testPattern);
        }
    }
}