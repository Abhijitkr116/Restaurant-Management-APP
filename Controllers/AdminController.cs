using RestaurantCopy.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantCopy.Controllers
{
    /*[Authorize(Roles = "Admin")]*/
    public class AdminController : Controller
    {
        RestaurantliteDataContext DataContext = new RestaurantliteDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddItems()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddItems(FoodItemModel foodItem)
        {
            FoodItem food = new FoodItem();
            food.ItemName = foodItem.ItemName;
            food.ItemTypeId = foodItem.FoodTypeId;
            food.Price = foodItem.Price;

            DataContext.FoodItems.InsertOnSubmit(food);
            DataContext.SubmitChanges();

            /*ViewBag.message = "Inserted Successfully";
            return View("Success");*/
            return RedirectToAction("GetItems");
        }
        
        public ActionResult UpdateItem(FoodItemModel foodItem)
        {
            var updated = (from i in DataContext.FoodItems where i.Id == foodItem.Id select i).FirstOrDefault();
            if (updated != null)
            {
                updated.ItemName = foodItem.ItemName;
                updated.ItemTypeId = foodItem.FoodTypeId;
                updated.Price = foodItem.Price;

                DataContext.SubmitChanges();
            }
            return RedirectToAction("GetItems");
        }

        public ActionResult EditDetails(int id)
        {
            var Items = (from i in DataContext.FoodItems
                         join
             t in DataContext.FoodTypes on i.ItemTypeId equals t.Id
                         where i.Id == id
                         select new FoodItemModel
                         {
                             Id = i.Id,
                             ItemName = i.ItemName,
                             FoodType = t.TypeName,
                             FoodTypeId = i.ItemTypeId,
                             Price = i.Price,
                         }).FirstOrDefault();
            return View(Items);
        }

        public ActionResult ViewBooking()
        {
            return View();        
        }

        public ActionResult GetItems()
        {
            var Items = (from i in DataContext.FoodItems
                         join
                         t in DataContext.FoodTypes on i.ItemTypeId equals t.Id
                         select new FoodItemModel
                         {
                             Id = i.Id,
                             ItemName = i.ItemName,
                             FoodType = t.TypeName,
                             Price = i.Price,
                         }).ToList();
            return View(Items);
        }
        // For Dropdown 
        [HttpGet]
        public JsonResult GetFoodTypes()
        {
            var foodTypes = (from i in DataContext.FoodTypes
                             select new
                             {
                                 i.Id,
                                 i.TypeName,
                             }).ToList();

            return Json(new { data = foodTypes }, JsonRequestBehavior.AllowGet);
        }

        

    }
}