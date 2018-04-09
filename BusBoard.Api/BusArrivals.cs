using System;
using System.Collections.Generic;

namespace BusBoard.ConsoleApp
{
    public class BusArrival
    {
        public string VehicleId { get; set; }
        public DateTime ExpectedArrival { get; set; }
    }

    public class BusPointId
    {
        public List<StopPoint> StopPoints { get; set; }
        public string CentrePoint { get; set; }
    }

    public class StopPoint
    {
        public string NaptanId { get; set; }
        public string CommonName { get; set; }
        public double Distance { get; set; }

        public List<LineGroup> LineGroup { get; set; }

    }

   public class LineGroup
    {
        public string NaptanIdReference { get; set; }
    }
}

