using BearerAuthentication.Client.Model;
using RestSharp;
using System;

namespace BearerAuthentication.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://localhost:61637/");

            var request = new RestRequest("api/security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "mkspinto");
            request.AddParameter("password", "mkspinto");

            IRestResponse<AuthenticationModel> response = client.Execute<AuthenticationModel>(request);
            var token = response.Data.access_token;

            request = new RestRequest("api/values", Method.GET);

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
