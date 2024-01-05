using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Flight;

namespace Presentation.Controllers
{
    public class FlightController : Controller
    {
        private FlightDbRepository _flightRepository;
        public FlightController(FlightDbRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public IActionResult ListFlights()
        {
            try
            {
                IQueryable<Flight> list = _flightRepository.GetFlights().OrderBy(x => x.CountryFrom);

                var output = from p in list
                             select new ListFlightsViewModel()
                             {
                                 Id = p.Id,
                                 Rows = p.Rows,
                                 Columns = p.Columns,
                                 DepartureDate = p.DepartureDate,
                                 ArrivalDate = p.ArrivalDate,
                                 CountryFrom = p.CountryFrom,
                                 CountryTo = p.CountryTo,
                                 WholesalePrice = p.WholesalePrice,
                                 CommissionRate = p.CommissionRate
                             };

                return View(output);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error while loading the flight data!";
                return RedirectToAction("Index", "Home");
            }
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
            if (ModelState.IsValid == false)
            {
                TempData["Error"] = "Error occured while creating a flight!";
                return View(myModel);
            }
            else
            {
                //{1a816378-8582-45d8-aed6-216ceb44e321}
                Guid newFlightId = Guid.NewGuid();


                _flightRepository.AddFlight(new Flight{
                        Id = newFlightId,
                        Rows = myModel.Rows,
                        Columns = myModel.Columns,
                        DepartureDate = myModel.DepartureDate,
                        ArrivalDate = myModel.ArrivalDate,
                        CountryFrom = myModel.CountryFrom,
                        CountryTo = myModel.CountryTo,
                        WholesalePrice = myModel.WholesalePrice,
                        CommissionRate = myModel.CommissionRate
                });

                TempData["message"] = "Flight saved successfully!";
                return RedirectToAction("ListFlights", "Flight");
            }
        }

        public IActionResult DeleteFlight(Guid Id)
        {
            try
            {
                _flightRepository.DeleteFlight(Id);
                TempData["message"] = "Product deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted";
            }

            return RedirectToAction("ListFlights", "Flight");
        }


        [HttpGet]
        public IActionResult EditFlight(Guid Id)
        {
            EditFlightViewModel flightModel = new EditFlightViewModel();


            //this is just to show current details of the flight to the user
            var originalFlight = _flightRepository.GetFlight(Id);
            flightModel.Rows = originalFlight.Rows;
            flightModel.Columns = originalFlight.Columns;
            flightModel.DepartureDate = originalFlight.DepartureDate;
            flightModel.ArrivalDate = originalFlight.ArrivalDate;
            flightModel.CountryFrom = originalFlight.CountryFrom;
            flightModel.CountryTo = originalFlight.CountryTo;
            flightModel.WholesalePrice = originalFlight.WholesalePrice;
            flightModel.CommissionRate = originalFlight.CommissionRate;

            return View(flightModel);
        }


        [HttpPost]
        public IActionResult EditFlight(EditFlightViewModel myModel)
        {
            if (_flightRepository.GetFlight(myModel.Id) == null)
            {
                TempData["error"] = "Flight does not exist";
                return RedirectToAction("ListFlights", "Flight");
            }
            else
                _flightRepository.UpdateFlight(new Flight()
                {
                    Id = myModel.Id,
                    Rows = myModel.Rows,
                    Columns = myModel.Columns,
                    DepartureDate = myModel.DepartureDate,
                    ArrivalDate = myModel.ArrivalDate,
                    CountryFrom = myModel.CountryFrom,
                    CountryTo = myModel.CountryTo,
                    CommissionRate = myModel.CommissionRate,
                    WholesalePrice = myModel.WholesalePrice,

                });
            TempData["message"] = "Product saved successfully!";
            return RedirectToAction("ListFlights", "Flight");

        }




        public IActionResult FlightDetails(Guid Id)
        {
            var chosenFlight = _flightRepository.GetFlight(Id);
            if (chosenFlight == null)
            {
                //add information that flight was not found
                TempData["error"] = "File was not found!";
                return RedirectToAction("ListFlights", "Flights");
            }
            else
            {

                ListFlightsViewModel flightModel = new ListFlightsViewModel()
                {
                    Id = chosenFlight.Id,
                    Rows = chosenFlight.Rows,
                    Columns = chosenFlight.Columns,
                    DepartureDate = chosenFlight.DepartureDate,
                    ArrivalDate = chosenFlight.ArrivalDate,
                    CountryFrom = chosenFlight.CountryFrom,
                    CountryTo = chosenFlight.CountryTo,
                };
                return View(flightModel);
            }
        }


    }
}
