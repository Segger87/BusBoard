using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().StartProgram();
        }

        private void StartProgram()
        {
            var userBusStopInput = BusStopInput();
            int userNumberOfBuses = NumberOfBusesInput();
            
            var busList = CallTflApi(userBusStopInput);
            var orderedBuses = AddBusesInList(busList);
            PrintResult(userNumberOfBuses, orderedBuses);
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

        public IRestResponse<List<BusArrivals>> CallTflApi(string userStopCode)
        {
            string stopCode = userStopCode;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var Client = new RestClient("https://api.tfl.gov.uk/StopPoint/" + stopCode);
            var request = new RestRequest("Arrivals", Method.GET);
            var getBusData = Client.Execute<List<BusArrivals>>(request);

            return getBusData;
        }

        public List<BusArrivals> AddBusesInList(IRestResponse<List<BusArrivals>> getBusData)
        {
            var busList = new List<BusArrivals>();

            foreach (var buses in getBusData.Data)
            {
                busList.Add(buses);
            }
            return busList;
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
