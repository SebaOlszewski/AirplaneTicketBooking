using Data.DataContext;
using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class FlightsController : Controller
    {
        private FlightDbRepository _airlineRepository;
        public FlightsController(FlightDbRepository flightRepository)
        {
            _airlineRepository = flightRepository;
        }

        //show flights
        public IActionResult ListFlights()
        {
            IQueryable<Flight> list = _airlineRepository.GetFlights().OrderBy(x => x.CountryFrom);

            var output = from p in list select new ListFlightsViewModel()
                            {
                                Id = p.Id,
                                Rows = p.Rows,
                                Columns = p.Columns,
                                DepartureDate = p.DepartureDate,
                                ArrivalDate = p.ArrivalDate,
                                CountryFrom = p.CountryFrom,
                                CountryTo = p.CountryTo,
                            };

            return View(output);

        }


        [HttpGet]
        public IActionResult CreateFlight()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult CreateFlights()
        {

            return RedirectToAction("Index", "Home");        
        }
    }
}
