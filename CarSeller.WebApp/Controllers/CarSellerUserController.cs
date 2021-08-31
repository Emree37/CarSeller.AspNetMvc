using CarSeller.BusinessLayer;
using CarSeller.BusinessLayer.Results;
using CarSeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarSeller.WebApp.Controllers
{
    public class CarSellerUserController : Controller
    {

        private CarSellerUserManager carSellerUserManager = new CarSellerUserManager();


        public ActionResult Index()
        {
            return View(carSellerUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CarSellerUser carSellerUser = carSellerUserManager.Find(x => x.Id == id.Value);

            if (carSellerUser == null)
            {
                return HttpNotFound();
            }

            return View(carSellerUser);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarSellerUser carSellerUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<CarSellerUser> res = carSellerUserManager.Insert(carSellerUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(carSellerUser);
                }

                return RedirectToAction("Index");
            }

            return View(carSellerUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CarSellerUser carSellerUser = carSellerUserManager.Find(x => x.Id == id.Value);

            if (carSellerUser == null)
            {
                return HttpNotFound();
            }

            return View(carSellerUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarSellerUser carSellerUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<CarSellerUser> res = carSellerUserManager.Update(carSellerUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(carSellerUser);
                }

                return RedirectToAction("Index");
            }
            return View(carSellerUser);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CarSellerUser carSellerUser = carSellerUserManager.Find(x => x.Id == id.Value);

            if (carSellerUser == null)
            {
                return HttpNotFound();
            }

            return View(carSellerUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarSellerUser carSellerUser = carSellerUserManager.Find(x => x.Id == id);
            carSellerUserManager.Delete(carSellerUser);

            return RedirectToAction("Index");
        }
    }
}