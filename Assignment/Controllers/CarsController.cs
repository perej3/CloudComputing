using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assignment.Data;
using Assignment.DataAccess.Interfaces;
using Assignment.DataAccess.Repositories;
using Assignment.Domain;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Assignment.Controllers
{
    [Authorize(Roles = "Driver")]
    public class CarsController : Controller
    {

        private readonly ICarsRepository  _carsRepo;
        private readonly IConfiguration _config;
        private readonly ILogRepository _log;
        
        public CarsController(ICarsRepository carsRepo, IConfiguration config, ILogRepository log) 
        {
            _carsRepo = carsRepo;
            _config = config;
            _log = log;
        }
        

        public IActionResult Index()
        {
            return View(_carsRepo.GetCars());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormFile image, Car c)
        {
            try
            {
                
                string bucketName = _config.GetSection("AppSettings").GetSection("PicturesBucket").Value;

                string filename = Guid.NewGuid() + Path.GetExtension(image.FileName);

                c.URL = $"https://storage.googleapis.com/{bucketName}/{filename}";

                var storage = StorageClient.Create();
                using (var fileToUpload = image.OpenReadStream())
                {
                    storage.UploadObject(bucketName, filename,  null, fileToUpload);
                }

                c.Driver = _carsRepo.GetDriver(User.Identity.Name);

                
                _carsRepo.InsertCar(c);


                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                _log.Log("Failed to upload Car");
                return View();
            }
        }



        
    }
}
