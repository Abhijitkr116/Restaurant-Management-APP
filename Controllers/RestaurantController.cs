using RestaurantCopy.Models;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace RestaurantCopy.Controllers
{
    public class RestaurantController : Controller
    {
        RestaurantliteDataContext DataContext = new RestaurantliteDataContext();

        // Restaurant : Homepage
        public ActionResult Index()
        {
            return View();
        }


        // Register Page - Register yourself

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel userdata)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                User user = new User();
                user.FullName = userdata.FullName;
                user.Email = userdata.Email;
                user.Username = userdata.Username;
                user.PasswordHash = HashPassword(userdata.PasswordHash);
                user.Phone = userdata.Phone;
                user.CreatedAt = DateTime.Now;
                DataContext.Users.InsertOnSubmit(user);
                DataContext.SubmitChanges();
                /*ViewBag.message = "Congrats, you're registered successfully";
                return View("Success");*/
                ViewData["Success"] = "SuccessAlert();";
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        // Login Page

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel loginModel)
        {
            if (loginModel == null)
            {
                ViewData["Error"] = "ErrorAlert();";
                ModelState.Clear();
                return View();
            }

            var user = DataContext.Users.FirstOrDefault(u => u.Username == loginModel.Username);
            if(loginModel.Username == "Admin" && VerifyPassword(loginModel.PasswordHash, user.PasswordHash))
            {
                //Successful login for Admin
                return RedirectToAction("Index", "Admin");
            }
            if (user != null && VerifyPassword(loginModel.PasswordHash, user.PasswordHash))
            {
                // Successful login for user
                Session["Username"] = user.FullName;
                return RedirectToAction("TableBooking", "Restaurant");
            }

            else
            {
                ViewData["Error"] = "ErrorAlert();";
                ModelState.Clear();
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }


        // Food Menu - List of the Food types with its price
        public ActionResult Menu()
        {
            return View();
        }



        // Table Booking - Book your table with ease

        [HttpGet]
        public ActionResult TableBooking()
        {
            if (Session["Username"] != null){
               return View();
            }
            else{
               return RedirectToAction("Login", "Restaurant");
            }
        }

        [HttpPost]
        public ActionResult TableBooking(BookingModel booktable)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Calculate end time with buffer (10 minutes buffer)
            TimeSpan bookingTime = booktable.BookingTime; // Assuming this is a TimeSpan
            int bufferTime = 10; // Buffer time in minutes

            // Assuming booktable.Duration is in minutes and is an int or a TimeSpan
            TimeSpan duration = TimeSpan.FromMinutes(booktable.Duration);

            // Calculate the EndTime
            TimeSpan endTime = bookingTime + duration + TimeSpan.FromMinutes(bufferTime);



            Booking book = new Booking()
            {
                UserId = 1,
                TableId = booktable.TableId,
                BookingDate = booktable.BookingDate,
                BookingStartTime = bookingTime,
                BookingEndTime = endTime,
                NumberOfGuests = booktable.NumberOfGuests,
                CreatedAt = DateTime.Now

            };

            DataContext.Bookings.InsertOnSubmit(book);
            DataContext.SubmitChanges();

            ViewBag.Message = "Table booked successfully!";
            return View("Success");

        }


        /*public ActionResult BookingHistory()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Restaurant");
            }

            string username = Session["Username"].ToString();
            var user = DataContext.Users.FirstOrDefault(u => u.FullName == username);

            if (user != null)
            {
                var bookings = DataContext.Bookings
                    .Where(b => b.UserId == user.UserId)
                    .Select(b => new BookingHistoryModel
                    {
                        BookingId = b.BookingId,
                        BookingDate = b.BookingDate,
                        BookingStartTime = b.BookingStartTime,
                        BookingEndTime = b.BookingEndTime,
                        NumberOfGuests = b.NumberOfGuests,
                        Status = b.IsVerified ? "Verified" : "Pending",
                        FormattedBookingStartTime = string.Format("{0:D2}:{1:D2}", b.BookingStartTime.Hours, b.BookingStartTime.Minutes),
                        FormattedBookingEndTime = string.Format("{0:D2}:{1:D2}", b.BookingEndTime.Hours, b.BookingEndTime.Minutes)
                    })
                    .ToList();

                return View(bookings);
            }

            return HttpNotFound("User not found.");
        }*/




        // Available tables - Bring the details of available table

        [HttpGet]
        public JsonResult GetAvailableTables(DateTime bookingDate, TimeSpan bookingTime, int duration)
        {
            // Calculate the end time of the requested booking
            TimeSpan bookingEndTime = bookingTime.Add(TimeSpan.FromMinutes(duration));

            // Fetch booked table IDs for the given date where the booking time overlaps with an existing booking
            var bookedTableIds = DataContext.Bookings
                .Where(b => b.BookingDate == bookingDate &&
                            ((b.BookingStartTime <= bookingTime && b.BookingEndTime > bookingTime) ||
                             (b.BookingStartTime < bookingEndTime && b.BookingEndTime >= bookingEndTime) ||
                             (b.BookingStartTime >= bookingTime && b.BookingEndTime <= bookingEndTime)))
                .Select(b => b.TableId)
                .ToList();

            // Fetch available tables that are not in the list of booked tables
            var availableTables = DataContext.Tables
                .Where(t => !bookedTableIds.Contains(t.TableId))
                .Select(t => new { t.TableId, t.TableNumber, t.Capacity })
                .ToList();

            return Json(availableTables, JsonRequestBehavior.AllowGet);
        }



        // Get All the Food Items for menu page
        [HttpGet] 
        public JsonResult getAllItems()
        {
            var Items = (from item in DataContext.FoodItems
                         select new
                         {
                             item.ItemTypeId,
                             item.ItemName,
                             item.Price
                         });

            return Json(new { data = Items }, JsonRequestBehavior.AllowGet);
        }






        //Some private methods 
        private ActionResult InternalServerError(Exception ex)
        {
            throw new NotImplementedException();
        }

        private ActionResult BadRequest(string v)
        {
            throw new NotImplementedException();
        }
        private ActionResult Unauthorized()
        {
            throw new NotImplementedException();
        }

        // It'll convert your password to HashPassword
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // It'll verify your HashPassword
        private bool VerifyPassword(string password, string storedHash)
        {
            string hash = HashPassword(password);
            return hash == storedHash;
        }

    }
}