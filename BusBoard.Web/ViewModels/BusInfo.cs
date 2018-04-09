using BusBoard.ConsoleApp;
using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
  public class BusInfoDisplay
  {
    public BusInfoDisplay(List<BusArrival> busArrivals)
    {
      BusArrivals = busArrivals;
    }

    public List<BusArrival> BusArrivals { get; set; }
  }
}
