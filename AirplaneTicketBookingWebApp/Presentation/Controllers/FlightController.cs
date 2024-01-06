using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Flight;

namespace Presentation.Controllers
{
    public class FlightController : Controller
    {
        private FlightDbRepository _flightRepository;
        private SeatDbRepository _seatRepository;
        public FlightController(FlightDbRepository flightRepository, SeatDbRepository seatRepository)
        {
            _flightRepository = flightRepository;
            _seatRepository = seatRepository;
        }

        public IActionResult ListFlights()
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                IQueryable<Flight> list = _flightRepository.GetFlights()
                    .Where(x => x.DepartureDate >= currentDate)
                    .OrderBy(x => x.CountryFrom);
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
                                 CommissionRate = p.CommissionRate,
                                 seatingList = _seatRepository.GetAllTheSeatsFromAFlight(p.Id).ToList()

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

        public IActionResult DeleteFlight(Guid Id, FlightDbRepository FlightRepositoryInjection)
        {
            try
            {
                FlightRepositoryInjection.DeleteFlight(Id);
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



            if (User.Identity.Name == "sebaolszewski39@gmail.com")
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
            else
            {
                TempData["error"] = "Permission not granted!!!";
                return RedirectToAction("Index", "Home");
            }
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
            return RedirectToAction("AdminListFlights", "Admin");

        }




        public IActionResult FlightDetails(Guid Id)
        {
            var chosenFlight = _flightRepository.GetFlight(Id);
            if (chosenFlight == null)
            {
                TempData["error"] = "Flight was not found!";
                return RedirectToAction("ListFlights", "Flights");
            }
            else
            {
                List<Seat> _seatingList = _seatRepository.GetAllTheSeatsFromAFlight(Id).ToList();
                ListFlightsViewModel flightModel = new ListFlightsViewModel()
                {
                    Id = chosenFlight.Id,
                    Rows = chosenFlight.Rows,
                    Columns = chosenFlight.Columns,
                    DepartureDate = chosenFlight.DepartureDate,
                    ArrivalDate = chosenFlight.ArrivalDate,
                    CountryFrom = chosenFlight.CountryFrom,
                    CountryTo = chosenFlight.CountryTo,
                    seatingList = _seatingList
                };
                return View(flightModel);
            }
        }


    }
}
