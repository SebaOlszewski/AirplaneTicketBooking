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
        private FlightDbRepository _flightRepository;
        public FlightsController(FlightDbRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        //show flights
        public IActionResult ListFlights()
        {
            IQueryable<Flight> list = _flightRepository.GetFlights().OrderBy(x => x.CountryFrom);

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
            CreateFlightViewModel myModel = new CreateFlightViewModel();
            return View(myModel);
        }

        [HttpPost]
        public IActionResult CreateFlight(CreateFlightViewModel myModel)
        {
           
                _flightRepository.AddFlight(new Flight()
                {
                    Rows = myModel.Rows,
                    Columns = myModel.Columns,
                    DepartureDate = myModel.DepartureDate,
                    ArrivalDate = myModel.ArrivalDate,
                    CountryFrom = myModel.CountryFrom,
                    CountryTo = myModel.CountryTo,

                });
                //TempData["message"] = "Product saved successfully!";
                return RedirectToAction("ListFlights", "Flights");
           
        }
    }
}
