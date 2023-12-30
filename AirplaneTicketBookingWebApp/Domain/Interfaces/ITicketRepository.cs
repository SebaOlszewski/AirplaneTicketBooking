using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {
        
        void Book(Ticket newTicket);

        void Cancel(Guid Id);


        Ticket? GetTickets(Guid ticketID);


        IQueryable<Ticket> GetTickets();


        void DeleteTicket(Guid ticketID);

        void updateTicket(Ticket ticket);
        

    }
}
