using Data.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketDbRepository
    {
        public AirlineDbContext _AirlineDbContext;
        public TicketDbRepository(AirlineDbContext AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }


        public void Book(Ticket newTicket)
        {
            _AirlineDbContext.Tickets.Add(newTicket);
            _AirlineDbContext.SaveChanges();

        }

        public void Cancel(Ticket chosenTicket)
        {
            var originalTicket = GetTickets(chosenTicket.Id);
            if(originalTicket != null)
            {    
                originalTicket.Cancelled = chosenTicket.Cancelled;
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                //Have to create a pop up box to inform user
                throw new Exception("Ticket does not exist");
            }
        }

        public Ticket? GetTickets(int  ticketID)
        {
            return _AirlineDbContext.Tickets.SingleOrDefault(x => x.Id == ticketID);
        }

    }
}
