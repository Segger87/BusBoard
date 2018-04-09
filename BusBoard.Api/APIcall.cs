using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BusBoard.ConsoleApp;

namespace BusBoard.Api
{
    public class APIcall
    {
        public BusPointId CallTflApi(LatLong latLong)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var findStopPoint = "StopPoint?stopTypes=NaptanOnstreetBusCoachStopPair&radius=500&lat=" + latLong.Latitude + "&lon=" + latLong.Longitude;
            var Client = new RestClient("https://api.tfl.gov.uk/");
            var request = new RestRequest(findStopPoint, Method.GET);
            var getBusData = Client.Execute<BusPointId>(request);

            return getBusData.Data;
        }

        public List<string> PrintStopPointId(BusPointId busList)
        {
            List<string> naptanIds = new List<string>();

            int i = 0;
            foreach (var stopPoint in busList.StopPoints)
            {
                var naptanIdReference = stopPoint.LineGroup.ElementAt(0).NaptanIdReference;
                naptanIds.Add(naptanIdReference);
                Console.WriteLine("Your nearest 2 StopPoint ID's are " + naptanIdReference + " Which is called " +
                    stopPoint.CommonName + " and it is located " + stopPoint.Distance + " meters away");
                i++;
                if (i == 2)
                {
                    break;
                }
            }
            Console.ReadLine();

            return naptanIds;
        }

        public List<BusArrival> GetUpComingBuses(List<string> NaptanIds)
        {
            var buses = new List<BusArrival>();
            foreach (var naptan in NaptanIds)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var Client = new RestClient("https://api.tfl.gov.uk/StopPoint/" + naptan);
                var request = new RestRequest("Arrivals", Method.GET);
                var busArrivalsResponse = Client.Execute<List<BusArrival>>(request);
                buses = buses.Concat(busArrivalsResponse.Data).ToList(); // concatenates seperate lists into one
            }
            return buses;
        }

        public void PrintResult(int UserNumberOfBuses, List<BusArrival> busList)
        {
            var totalBuses = busList.Count;
            if (UserNumberOfBuses > totalBuses)
            {
                UserNumberOfBuses = totalBuses;
                Console.WriteLine("Sorry Bruv I will give you " + UserNumberOfBuses + " instead");
            }

            List<BusArrival> SortedList = busList.OrderBy(o => o.ExpectedArrival).ToList();

            for (int i = 0; i < UserNumberOfBuses; i++)
            {
                Console.WriteLine("The following bus with the ID " + UserNumberOfBuses + SortedList.ElementAt(i).VehicleId + " will be arriving at " + SortedList.ElementAt(i).ExpectedArrival.ToLocalTime().TimeOfDay);
            }
            Console.ReadLine();
        }
    }
}
