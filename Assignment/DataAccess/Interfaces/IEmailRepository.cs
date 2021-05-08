using Assignment.Domain;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Interfaces
{
    public interface IEmailRepository
    {
       IRestResponse SendSimpleMessage(string email, string message);

        string GetApiKey();
    }
}
