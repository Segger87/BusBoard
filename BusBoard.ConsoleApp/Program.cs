using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BusBoard.Api;

namespace BusBoard.ConsoleApp
{
    public class Program
    {

        static void Main(string[] args)
        {
            new Program().StartProgram();
        }

        private void StartProgram()
        {
            var apiCall = new APIcall();
            var postCodeApi = new PostCodeAPI();

            Console.WriteLine("Please enter a postcode: ");
            var postcode = Console.ReadLine();

            var userLatLong = postCodeApi.GetLatLong(postcode);

            //var userBusStopInput = BusStopInput();
            int userNumberOfBuses = NumberOfBusesInput();
            var busList = apiCall.CallTflApi(userLatLong);
            var naptanIds = apiCall.PrintStopPointId(busList);
            var busArrivals = apiCall.GetUpComingBuses(naptanIds);
        }

        public List<BusArrival> CheckPostcodeRegEx(string postcode)
        {
            string regexp = "([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|" +
          "(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z]" +
          @"[0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})";
            Match match = Regex.Match(postcode, regexp);
            if (match.Success)
            {
                return BusArrivalsFromPostcode(postcode);
            }
            else
            {
                return null;
            }
        }
        public List<BusArrival> BusArrivalsFromPostcode(string postcode)
        {

                var postCodeApi = new PostCodeAPI();
                var apiCall = new APIcall();
                var userLatLong = postCodeApi.GetLatLong(postcode);

                var busList = apiCall.CallTflApi(userLatLong);
                var naptanIds = apiCall.PrintStopPointId(busList);
                var busArrivals = apiCall.GetUpComingBuses(naptanIds);

                return busArrivals;
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
    }
}