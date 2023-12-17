using Data.DataContext;
using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using Presentation.Models.ViewModels.Tickets;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private TicketDbRepository _ticketRepository;
        private FlightDbRepository _flightsRepository;
        public TicketsController(TicketDbRepository ticketRepository, FlightDbRepository flightsRepository)
        {
            _ticketRepository = ticketRepository;
            _flightsRepository = flightsRepository;
        }

        public IActionResult ListTickets()
        {
            IQueryable<Ticket> list = _ticketRepository.GetTickets().OrderBy(x => x.Id);
            var output = from p in list
                         select new ListTicketsViewModel()
                         {
                             Id = p.Id,
                             Row = p.Row,
                             Column = p.Column,
                             FlightFK = p.FlightFK,
                             Passport = p.Passport,
                             PricePaid = p.PricePaid,
                             Cancelled = p.Cancelled,
                         };
            return View(output);
        }

        [HttpGet]
        public IActionResult Book()
        {
            BookTicketViewModel myModel = new BookTicketViewModel(_flightsRepository);
            return View(myModel);
        }


        [HttpPost]
        public IActionResult Book(BookTicketViewModel myModel)
        {
            try
            {
                _ticketRepository.Book(new Ticket()
                {
                    FlightFK = myModel.FlightFK,
                    Row = myModel.Row,
                    Column = myModel.Column,
                    Passport = myModel.Passport,
                    PricePaid = myModel.PricePaid
                });
                return RedirectToAction("ListTickets", "Tickets");
            }catch (Exception ex)
            {
                myModel.Flights = _flightsRepository.GetFlights();
                //TempData["message"] = "Product saved successfully!";
                return RedirectToAction("ListTickets", "Tickets");
            }
        }

        public IActionResult Cancel(Guid Id)
        {
            try
            {
                _ticketRepository.Cancel(Id);
                
            }catch(Exception ex)
            {
                TempData["error"] = "Ticket was not canceled";
            }

            return RedirectToAction("ListTickets", "Tickets");
        }

        public IActionResult DeleteTicket(Guid Id)
        {
            try
            {
                _ticketRepository.Cancel(Id);
                TempData["message"] = "Product deleted successfully";
                _ticketRepository.DeleteTicket(Id);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted";
            }
            return RedirectToAction("ListTickets", "Tickets");
        }



    }
}
