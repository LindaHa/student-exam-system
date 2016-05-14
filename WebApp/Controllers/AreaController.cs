using BusinessLayer.DTO;
using BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AreaController : Controller
    {
        AreaFacade areaFacade = new AreaFacade();
          
        public ActionResult Show(int id)
        {
            var area = areaFacade.GetAreaById(id);
            return View(area);
        }

        public ActionResult Index()
        {
            ViewBag.Warning = TempData["AreaTextWarning"];
            var model = areaFacade.GetAllArea();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new AreaDTO());
        }

        [HttpPost]
        public ActionResult Create(AreaDTO area)
        {
            if (ModelState.IsValid)
            {
                AreaDTO newArea = new AreaDTO();
                newArea.Id = area.Id;
                newArea.Name = area.Name;
                newArea.Questions = area.Questions;
                newArea.TestPatterns = area.TestPatterns;

                areaFacade.CreateArea(area);
                return RedirectToAction("Index");
            }
            return View(area);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                areaFacade.DeleteArea(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData.Add("AreaTextWarning", ex.Message);
            }
            return RedirectToAction("Index");


        }

        public ActionResult Edit(int id)
        {
            var area = areaFacade.GetAreaById(id);
            return View(area);
        }

        [HttpPost]
        public ActionResult Edit(int id, AreaDTO area)
        {
            if (ModelState.IsValid)
            {
                var originalArea = areaFacade.GetAreaById(id);
                originalArea.Name = area.Name;

                areaFacade.ModifyArea(originalArea);

                return RedirectToAction("Show", new { id = originalArea.Id });
            }
            return View(area);
        }
    }
}