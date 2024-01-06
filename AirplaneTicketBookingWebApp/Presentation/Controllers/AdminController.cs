using Data.Repositories;
using Domain.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Admin;
using Presentation.Models.ViewModels.Flight;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private FlightDbRepository _flightRepository;
        private SeatDbRepository _seatRepository;
        private TicketDbRepository _ticketRepository;
        public AdminController(FlightDbRepository flightRepository, SeatDbRepository seatRepository, TicketDbRepository ticketRepository)
        {
            _flightRepository = flightRepository;
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
        }



        public IActionResult AdminListFlights()
        {
            if (User.Identity.Name == "sebaolszewski39@gmail.com")
           {
                   try
                    {
                        IQueryable<Flight> list = _flightRepository.GetFlights().OrderBy(x => x.CountryFrom);
                        var output = from p in list
                                     select new AdminListFlightsViewModel()
                                     {
                                         Id = p.Id,
                                         DepartureDate = p.DepartureDate,
                                         ArrivalDate = p.ArrivalDate,
                                         CountryFrom = p.CountryFrom,
                                         CountryTo = p.CountryTo,
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
            else
            {
                TempData["error"] = "Permission not granted!!!";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AdminListSeats(Guid chosenFlight)
        {
            if (User.Identity.Name == "sebaolszewski39@gmail.com")
            {
            try
            {
                DateTime currentDate = DateTime.Now;

                List<Seat> list = _seatRepository.GetSeats().Where(x => x.FlightFk == chosenFlight).ToList();



                AdminListSeatsViewModel displaySeatsModel = new AdminListSeatsViewModel()
                {
                    chosenFlight = chosenFlight,
                    maxRowLength = _flightRepository.GetFlight(chosenFlight).Rows,
                    maxColLength = _flightRepository.GetFlight(chosenFlight).Rows,
                    seatingList = list
                };


                return View(displaySeatsModel);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error while loading the seat data!";
                return RedirectToAction("AdminListFlights", "Admin");
            }
            }
            else
            {
                TempData["error"] = "Permission not granted!!!";
                return RedirectToAction("Index", "Home");
            }


        }


        public IActionResult AdminTicketDetails(Guid seatId)
        {
            if (User.Identity.Name == "sebaolszewski39@gmail.com")
            {
                var chosenSeat = _seatRepository.GetSeat(seatId);
                List<Ticket> ticketList = _ticketRepository.getTickets().Where(x => x.SeatFk == seatId).ToList();

                if (ticketList.Count == 0 && chosenSeat != null)
                {
                    TempData["error"] = "This seat had never beed taken, no tickets to display";
                    return RedirectToAction("AdminListSeats", "Admin", new { chosenFlight = chosenSeat.FlightFk });
                }
                if (ticketList == null )
                {

                    TempData["error"] = "File was not found!";
                    return RedirectToAction("AdminListSeats", "Admin", new { chosenFlight = chosenSeat.FlightFk });
                }
                else
                {
                    AdminListTicketDetailsViewModel flightModel = new AdminListTicketDetailsViewModel()
                    {
                        ticketList = ticketList,
                    };
                    return View(flightModel);
                }
            }
            else
            {
                TempData["error"] = "Permission not granted!!!";
                return RedirectToAction("Index", "Home");
            }



        }
    }
}
