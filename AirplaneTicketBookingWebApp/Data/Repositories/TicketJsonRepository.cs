using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketJsonRepository : ITickets
    {


        private string _path;
        private FlightDbRepository _flightDbRepository;
        public TicketJsonRepository(string path)
        {
            _path = path;
            if (File.Exists(_path) == false)
            {
                File.Create(_path).Close();
            }

        }


        public void Book(Ticket newTicket)
        {
            newTicket.Id = Guid.NewGuid();
            var list = GetTickets().ToList();
            list.Add(newTicket);

            var allFLightsText = JsonSerializer.Serialize(list);

            try
            {
                File.WriteAllText(_path, allFLightsText);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while booking a ticket");
            }
        }

        public void Cancel(Guid Id)
        {
            var list = GetTickets().ToList();
            var existingTicket = list.FirstOrDefault(f => f.Id == Id);

            if (existingTicket != null)
            {
                if (existingTicket.Cancelled == false)
                {
                    existingTicket.Cancelled = true;
                }else
                {
                    existingTicket.Cancelled = false;
                }
                try
                {
                    var allTicketsText = JsonSerializer.Serialize(list);
                    File.WriteAllText(_path, allTicketsText);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while canceling a ticket");
                }
            }
            else
            {
                throw new Exception($"Ticket not found");
            }
        }

        public void DeleteTicket(Guid ticketID)
        {
            throw new NotImplementedException();
        }

        public Ticket? GetTickets(Guid ticketID)
        {
            return GetTickets().SingleOrDefault(x => x.Id == ticketID);
        }

        public IQueryable<Ticket> GetTickets()
        {

            //checking if file exists
            if (File.Exists(_path))
            {
                try
                {

                    string allText = "";
                    //reading the data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    //return empty list if file was empty
                    if (string.IsNullOrEmpty(allText))
                    {
                        return new List<Ticket>().AsQueryable();
                    }
                    else
                    {

                        List<Ticket> myFlights = JsonSerializer.Deserialize<List<Ticket>>(allText); //converts from json string to an object
                        return myFlights.AsQueryable();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Error while opening the file");
                }
            }
            else throw new Exception("File saving products not found!");
        }


        public void updateTicket(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }

}
