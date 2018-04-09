using RestSharp;
using System.Net;


namespace BusBoard.Api
{
   public class PostCodeAPI
    {

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
