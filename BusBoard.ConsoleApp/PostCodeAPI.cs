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
        //public void CallPostCodeApi(string postcode)
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    var Client = new RestClient("http://api.postcodes.io/");
        //    var request = new RestRequest("postcodes/" + postcode, Method.GET);
        //    var getPostCodeData = Client.Execute<PostCodeAPIResponse>(request);
        //    var userLongitude = getPostCodeData.Data.Result.Longitude;
        //    var userLatitude = getPostCodeData.Data.Result.Latitude;

        //    Console.WriteLine(getPostCodeData.Data.Result.Postcode + " " + userLongitude + " " + userLatitude);

        //    Console.ReadLine();
        //}

        public LatLong GetLatLong(string postcode)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var Client = new RestClient("http://api.postcodes.io/");
            var request = new RestRequest("postcodes/" + postcode, Method.GET);
            var postCodeData = Client.Execute<PostCodeAPIResponse>(request);
            return postCodeData.Data.Result;
        }
    }

    public class PostCodeAPIResponse
    {
        public LatLong Result { get; set; }
    }

    public class LatLong
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
