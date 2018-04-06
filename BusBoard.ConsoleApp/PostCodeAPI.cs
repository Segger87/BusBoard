using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.ConsoleApp
{
    class PostCodeAPI
    {
        

        public void CallPostCodeApi(string postcode)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var Client = new RestClient("http://api.postcodes.io/");
            var request = new RestRequest("postcodes/" + postcode, Method.GET);
            var getPostCodeData = Client.Execute<PostCodeAPIResponse>(request);

            Console.WriteLine(getPostCodeData.Data.Result.Postcode);
       
            Console.ReadLine();
        }
    }

    public class PostCodeAPIResponse
    {
        public PostCode Result { get; set; }
    }

    public class PostCode
    {
        public string Postcode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
