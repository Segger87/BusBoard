using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        private PostCodeAPI postCodeApi;
        static void Main(string[] args)
        {
            new Program().StartProgram();
        }

        private void StartProgram()
        {
            postCodeApi = new PostCodeAPI();

            Console.WriteLine("Please enter a postcode: ");
            var postcode = Console.ReadLine();

            var userLatLong = postCodeApi.GetLatLong(postcode);

            //var userBusStopInput = BusStopInput();
            int userNumberOfBuses = NumberOfBusesInput();

            var busList = CallTflApi(userLatLong);
            var naptanIds = PrintStopPointId(busList);
            var busArrivals = GetUpComingBuses(naptanIds);
            PrintResult(userNumberOfBuses, busArrivals);
        }

        //private string BusStopInput()
        //{
        //    string busStopInput;
        //    do
        //    {
        //        Console.WriteLine("Which stopcode do you want to know the bus time for?");
        //        busStopInput = Console.ReadLine();
        //    } while (!BusStopInputInputIsValid(busStopInput));
        //    return busStopInput;
        //}

        private bool BusStopInputInputIsValid(string needsinput)
        {
            //Lookup into reg exp to find out valid stop codes
            return true;
        }

        private int NumberOfBusesInput()
        {
            string numberOfBusesInput;
            do
            {
                Console.WriteLine("How many buses would you like to know are coming up?");
                numberOfBusesInput = Console.ReadLine();
            } while (!NumberOfBusesInputIsValid(numberOfBusesInput));
            return int.Parse(numberOfBusesInput);
        }

        private bool NumberOfBusesInputIsValid(string userInput)
        {
            if (int.TryParse(userInput, out int n))
            {
                // todo now if else on it being more than 0
                return true;
            }
            else
            {
                // todo tell user its not a string
                return false;
            };
        }

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
                if(i == 2)
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
            foreach(var naptan in NaptanIds)
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
