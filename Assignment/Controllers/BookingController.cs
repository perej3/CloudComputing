using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.DataAccess.Interfaces;
using Assignment.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Assignment.Controllers
{
    
    public class BookingController : Controller
    {

        private readonly IBookingRepository _bookingsRepo;
        private readonly IConfiguration _config;
        private readonly IPubSubRepository _pubSub;
        private readonly ILogRepository _log;
        private readonly IEmailRepository _email;

        public BookingController(IBookingRepository bookingsRepo, IConfiguration config, IPubSubRepository pubSub, ILogRepository log, IEmailRepository email)
        {
            _bookingsRepo = bookingsRepo;
            _config = config;
            _pubSub = pubSub;
            _log = log;
            _email = email;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_bookingsRepo.GetBookings());
        }
        [Authorize(Roles = "Passenger")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Passenger")]
        [HttpPost]
        public IActionResult Create(Booking b)
        {
            try {

                //b.BookingId = 0;
                b.BookingId = _bookingsRepo.GetBookings().Count() + 1;
                b.Passenger = _bookingsRepo.GetPassenger(User.Identity.Name);
                _bookingsRepo.InsertBooking(b);

                
                string message = "Booking Id: 1\nPassenger: " + b.Passenger + "\n Location: " + b.longitude + ", " + b.latitude + "\nDestination: "+b.longitudeTo+", " + b.latitudeTo;


                IRestResponse response =  _email.SendSimpleMessage(b.Passenger.Email, message);

                
                // _pubSub.PublishEmail(User.Identity.Name, b, b.CategoryId);

            }
            catch(Exception e)
            {
                _log.Log("Error occurred in the booking process");
            }




            return RedirectToAction("Index");

        }

        
    }
}
