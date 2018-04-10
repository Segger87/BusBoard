using System.Web.Mvc;
using BusBoard.Web.Models;
using BusBoard.Web.ViewModels;
using BusBoard.ConsoleApp;

namespace BusBoard.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BusInfo(PostcodeSelection selection)
        {
            // Add some properties to the BusInfo view model with the data you want to render on the page.
            // Write code here to populate the view model with info from the APIs.
            // Then modify the view (in Views/Home/BusInfo.cshtml) to render upcoming buses.

            var busArrivals = new Program().CheckPostcodeRegEx(selection.Postcode);

            if (busArrivals == null)
            {
                return View("Error");
            }
            var info = new BusInfoDisplay(busArrivals);
          
            return View(info);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Information about this site";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact us!";

            return View();
        }
    }
}