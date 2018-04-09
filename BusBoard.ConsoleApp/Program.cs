using System;
using BusBoard.Api;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        //private APIcall apiCall;
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
            apiCall.PrintResult(userNumberOfBuses, busArrivals);
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