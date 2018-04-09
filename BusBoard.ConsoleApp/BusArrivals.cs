using System;
using System.Collections.Generic;

namespace BusBoard.ConsoleApp
{
    class BusArrival
    {
        public string VehicleId { get; set; }
        public DateTime ExpectedArrival { get; set; }
    }

    class BusPointId
    {
        public List<StopPoint> StopPoints { get; set; }
        public string CentrePoint { get; set; }
    }

    class StopPoint
    {
        public string NaptanId { get; set; }
        public string CommonName { get; set; }
        public double Distance { get; set; }

        public List<LineGroup> LineGroup { get; set; }

    }

    class LineGroup
    {
        public string NaptanIdReference { get; set; }
    }
}

