using Assignment.Domain;
using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Interfaces
{
    public interface IPubSubRepository
    {
        void PublishEmail(string email, Booking b, int categoryId);
        
    }
}
