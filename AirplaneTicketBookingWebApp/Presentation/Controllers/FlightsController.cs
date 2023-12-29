using Data.DataContext;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class FlightsController : Controller
    {
        private IFlights _flightRepository;


        public FlightsController(IFlights flightRepository)
        {
            _flightRepository = flightRepository;
        }


        //show flights
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
                             };

                return View(output);
            }catch (Exception ex)
            {
                TempData["error"] = "Error with loading the file!";
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

            _flightRepository.AddFlight(new Flight()
            {
                Rows = myModel.Rows,
                Columns = myModel.Columns,
                DepartureDate = myModel.DepartureDate,
                ArrivalDate = myModel.ArrivalDate,
                CountryFrom = myModel.CountryFrom,
                CountryTo = myModel.CountryTo,
                

            }) ;
                //TempData["message"] = "Product saved successfully!";
                return RedirectToAction("ListFlights", "Flights");
           
        }

        public IActionResult FlightDetails(Guid Id)
        {
            var chosenFlight = _flightRepository.GetFlights(Id);
            if(chosenFlight == null)
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

        public IActionResult DeleteFlight(Guid Id)
        {
            try
            {
                TempData["message"] = "Product deleted successfully";
                _flightRepository.DeleteFlight(Id);
            }catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted";
            }

            return RedirectToAction("ListFlights", "Flights");

        }


        [HttpGet]
        public IActionResult EditFlight(Guid Id)
        {
            EditFlightViewModel flightModel = new EditFlightViewModel();


            //this is just to show current details of the flight to the user
            var originalFlight = _flightRepository.GetFlights(Id);
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
           if(_flightRepository.GetFlights(myModel.Id) == null)
            {
                TempData["error"] = "Product does not exist";
                return RedirectToAction("ListFlights", "Flights");
            }else
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
            //TempData["message"] = "Product saved successfully!";
            return RedirectToAction("ListFlights", "Flights");

        }

    }
}
