using Assignment.DataAccess.Interfaces;
using Google.Cloud.SecretManager.V1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public string GetApiKey()
        {
                SecretManagerServiceClient client = SecretManagerServiceClient.Create();

                SecretVersionName secretVersionName = new SecretVersionName("pristine-abacus-307313", "ClientCredentials", "1");

                AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

                string payload = result.Payload.Data.ToStringUtf8();

                JsonConvert.DeserializeObject(payload);

                JObject jsonObject = JObject.Parse(payload);
                JToken jsonKey = jsonObject["MailGun"];

                return jsonKey.ToString();

        }

        public IRestResponse SendSimpleMessage(string email, string message)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                new HttpBasicAuthenticator("api",GetApiKey());
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandboxbe193ab0ed4d45f6bba142c4eec9d2d0.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "robert.cini.b31271@mcast.edu.mt");
            request.AddParameter("to", email);
            request.AddParameter("subject", "Booking receipt (no-reply)");
            request.AddParameter("text", message);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}
