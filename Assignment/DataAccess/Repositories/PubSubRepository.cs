using Assignment.DataAccess.Interfaces;
using Assignment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Configuration;
using Grpc.Core;
using Newtonsoft.Json;
using Google.Protobuf;

namespace Assignment.DataAccess.Repositories
{
    public class PubSubRepository : IPubSubRepository
    {
        string projectId;
        string topicId;
        private TopicName topicName;

        public PubSubRepository(IConfiguration config)
        {
            projectId = config.GetSection("AppSettings").GetSection("ProjectId").Value;
            topicId = config.GetSection("AppSettings").GetSection("TopicId").Value;
        }


        private Topic CreateTopic()
        {

            PublisherServiceApiClient publisher = PublisherServiceApiClient.Create();
            var topicName = TopicName.FromProjectTopic(projectId, topicId);
            Topic topic = null;

            try
            {
                topic = publisher.CreateTopic(topicName);
            }
            catch (RpcException e) 
            when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                topic = publisher.GetTopic(topicId);
            }

            return topic;
        }



        public void PublishEmail(string email, Booking b, int categoryId)
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
            topicId = "test";
            
            topicName = TopicName.FromProjectTopic(projectId, topicId);

            Task<PublisherClient> task = PublisherClient.CreateAsync(topicName);
            task.Wait();
            PublisherClient publisher = task.Result;

            var spontObject = new { email = email, booking = b };
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

