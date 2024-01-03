﻿using Data.DataContext;
using Domain.Models;
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


        public void cancelTicket(Ticket chosenTicket)
        {
            var ticketToCancel = getTicket(chosenTicket.Id);
            if (ticketToCancel != null)
            {
                ticketToCancel.Cancelled = true;
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No ticket to cancel!");
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
    }
}
