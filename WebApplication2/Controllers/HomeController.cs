using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            PublishEmail();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void PublishEmail()
        {

            /*switch (categoryId)
            {
                case 1:
                    topicId = "test";                 
                    break;
                case 2:
                    topicId = "BusinessTopic";                 
                    break;
                case 3:
                    topicId = "LuxuryTopic";                
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }*/
            var topicId = "test";

            var topicName = TopicName.FromProjectTopic("pfc2021", topicId);

            Task<PublisherClient> task = PublisherClient.CreateAsync(topicName);
            task.Wait();
            PublisherClient publisher = task.Result;

            var spontObject = new { email = "test", booking = "test" };
            string serializeSpontObject = JsonConvert.SerializeObject(spontObject);

            var pubsubMessage = new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8(serializeSpontObject)

            };

            Task<string> taskPublish = publisher.PublishAsync(pubsubMessage);
            taskPublish.Wait();


            string message = taskPublish.Result;

        }
    }
}
