using CarSeller.BusinessLayer;
using CarSeller.Entities;
using CarSeller.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarSeller.WebApp.Controllers
{
    public class CarController : Controller
    {

        private CarManager carManager = new CarManager();
        private CategoryManager categoryManager = new CategoryManager();
        private LikeManager likeManager = new LikeManager();


        public ActionResult Index()
        {
            var cars = carManager.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                x => x.ModifiedOn);

            return View(cars.ToList());
        }


        
        public ActionResult MyLikedNotes()
        {
            var cars = likeManager.ListQueryable().Include("LikedUser").Include("Car").Where(
                x => x.LikedUser.Id == CurrentSession.User.Id).Select(
                x => x.Car).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

            return View("Index", cars.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carManager.Find(x => x.Id == id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                car.Owner = CurrentSession.User;
                carManager.Insert(car);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", car.CategoryId);
            return View(car);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carManager.Find(x => x.Id == id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", car.CategoryId);
            return View(car);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Car db_car = carManager.Find(x => x.Id == car.Id);
                db_car.IsDraft = car.IsDraft;
                db_car.CategoryId = car.CategoryId;
                db_car.Text = car.Text;
                db_car.Title = car.Title;

                carManager.Update(db_car);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", car.CategoryId);
            return View(car);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carManager.Find(x => x.Id == id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = carManager.Find(x => x.Id == id);
            carManager.Delete(car);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                int userid = CurrentSession.User.Id;
                List<int> likedCarIds = new List<int>();

                if (ids != null)
                {
                    likedCarIds = likeManager.List(
                        x => x.LikedUser.Id == userid && ids.Contains(x.Car.Id)).Select(
                        x => x.Car.Id).ToList();
                }
                else
                {
                    likedCarIds = likeManager.List(
                        x => x.LikedUser.Id == userid).Select(
                        x => x.Car.Id).ToList();
                }

                return Json(new { result = likedCarIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        [HttpPost]
        public ActionResult SetLikeState(int carid, bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Like like =
                likeManager.Find(x => x.Car.Id == carid && x.LikedUser.Id == CurrentSession.User.Id);

            
            Car car = carManager.Find(x => x.Id == carid);

            if (like != null && liked == false)
            {
                res = likeManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likeManager.Insert(new Like()
                {
                    LikedUser = CurrentSession.User,
                    Car = car
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    car.LikeCount++;
                }
                else
                {
                    car.LikeCount--;
                }

                res = carManager.Update(car);

                return Json(new { hasError = false, errorMessage = string.Empty, result = car.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = car.LikeCount });
        }


        public ActionResult GetCarText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Car car = carManager.Find(x => x.Id == id);

            if (car == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialCarText", car);
        }
    }
}