using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITicketInterface
    {
        void book(Ticket newTicket);
        Ticket? getTicket(Guid ticketIt);
        IQueryable<Ticket> getTickets();
        void cancelTicket(Guid chosenTicketId);


        void updateTicket(Ticket chosenTicket);


        void DeleteTicket(Guid ticketID);
        
    }
}
