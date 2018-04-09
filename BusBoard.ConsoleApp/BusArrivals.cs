using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.ConsoleApp
{
    class BusArrivals
    {
        public string VehicleId { get; set; }
        public DateTime ExpectedArrival { get; set; }
    }

    class BusPointId
    {
        public string CentrePoint { get; set; }
        public string StopPoints { get; set; }
        public string NaptanId { get; set; }
    }
}
