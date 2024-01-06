using Data.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories 
{
    public class TicketDbRepository : ITicketInterface
    {
        public AirlineDbContext _AirlineDbContext;
        public SeatDbRepository _seatDbRepository;
        public FlightDbRepository _FlightDbRepository;
        public TicketDbRepository(AirlineDbContext AirlineDbContext, SeatDbRepository seatDbRepository, FlightDbRepository FlightDbRepository)
        {
            _AirlineDbContext = AirlineDbContext;
            _seatDbRepository = seatDbRepository;
            _FlightDbRepository = FlightDbRepository;
        }

        public void book(Ticket newTicket)
        {
            _AirlineDbContext.Tickets.Add(newTicket);
            _AirlineDbContext.SaveChanges();
        }


        public Ticket? getTicket(Guid ticketIt)
        {
            return getTickets().SingleOrDefault(x => x.Id == ticketIt);
        }

        public IQueryable<Ticket> getTickets()
        {
            return _AirlineDbContext.Tickets;
        }

        

        public void cancelTicket(Guid chosenTicketId)
        {
            var ticketToCancel = getTicket(chosenTicketId);
            if (ticketToCancel != null && ticketToCancel.Cancelled == false)
            {
                ticketToCancel.Cancelled = true;
                _AirlineDbContext.SaveChanges();
            }            
        }

        public void updateTicket(Ticket chosenTicket)
        {
            var ticketToCancel = getTicket(chosenTicket.Id);
            if (ticketToCancel != null)
            {
                ticketToCancel.SeatFk = chosenTicket.SeatFk;
                ticketToCancel.PassportImage = chosenTicket.PassportImage;
                ticketToCancel.PricePaid = chosenTicket.PricePaid;
                ticketToCancel.Cancelled = chosenTicket.Cancelled;
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No ticket to update!");
            }

        }

        public void DeleteTicket(Guid ticketID)
        {
            var ticketToDelete = getTicket(ticketID);
            if (ticketToDelete != null)
            {
                _AirlineDbContext.Remove(ticketToDelete);
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No ticket to delete!");
            }
        }
    }
}
