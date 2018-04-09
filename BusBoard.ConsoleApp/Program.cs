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

            var postcode = Console.ReadLine();
    
            //postCodeApi.CallPostCodeApi(postcode);
            string userLatitude = postCodeApi.GetLatitude(postcode);
            string userLongitude = postCodeApi.GetLongitude(postcode);

            var userBusStopInput = BusStopInput();
            int userNumberOfBuses = NumberOfBusesInput();
            
            var busList = CallTflApi(userLongitude, userLatitude);
            var orderedBuses = AddBusesInList(busList);
            PrintStopPointId(orderedBuses);
           // PrintResult(userNumberOfBuses, orderedBuses);
        }

        private string BusStopInput()
        {
            string busStopInput;
            do
            {
                Console.WriteLine("Which stopcode do you want to know the bus time for?");
                busStopInput = Console.ReadLine();
            } while (!BusStopInputInputIsValid(busStopInput));
            return busStopInput;
        }

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
            if (int.TryParse(userInput, out int n)) {
                // todo now if else on it being more than 0
                return true;
            } else {
                // todo tell user its not a string
                return false;
            };
        }

        public IRestResponse<List<BusPointId>> CallTflApi(string userLongitude, string userLatitude)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var findStopPoint = "StopPoint?stopTypes=NaptanOnstreetBusCoachStopPair&radius=500&lat=" + userLatitude + "&lon=" + userLongitude;
            var Client = new RestClient("https://api.tfl.gov.uk/");
            var request = new RestRequest(findStopPoint, Method.GET);
            var getBusData = Client.Execute<List<BusPointId>>(request);

            return getBusData;
        }

        public List<BusPointId> AddBusesInList(IRestResponse<List<BusPointId>> getBusData)
        {
            var busList = new List<BusPointId>();

            foreach (var buses in getBusData.Data)
            {
                busList.Add(buses);
            }
            return busList;
        }

        public void PrintStopPointId(List<BusPointId> busList)
        {
            for (int i = 0; i < busList.Count; i++)
            {
                Console.WriteLine("Your StopPoint ID is" + busList.ElementAt(i).StopPoints + busList.ElementAt(i).NaptanId);
            }
            Console.ReadLine();
        }

        public void PrintResult(int UserNumberOfBuses, List<BusArrivals> busList)
        {
           
            string space = " ";
            var totalBuses = busList.Count;
            if (UserNumberOfBuses > totalBuses)
            {
                UserNumberOfBuses = totalBuses;
                Console.WriteLine("Sorry Bruv I will give you " + UserNumberOfBuses + " instead");    
            }

            List<BusArrivals> SortedList = busList.OrderBy(o => o.ExpectedArrival).ToList();

            for (int i = 0; i < UserNumberOfBuses; i++)
            {
                Console.WriteLine("The following " + UserNumberOfBuses + SortedList.ElementAt(i).VehicleId + space + SortedList.ElementAt(i).ExpectedArrival.ToLocalTime().TimeOfDay);
            }
            Console.ReadLine();
        }
    }
}
